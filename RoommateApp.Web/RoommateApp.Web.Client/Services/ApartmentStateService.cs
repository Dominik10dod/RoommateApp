using Microsoft.JSInterop;
using RoommateApp.Web.Client;

namespace RoommateApp.Web.Client.Services
{
    public class ApartmentStateService
    {
        private readonly ApartmentClient _apartmentClient;
        private readonly IJSRuntime _js;

        public int Id { get; private set; }
        public string ApartmentName { get; set; } = "No room selected";
        public string Name => ApartmentName;
        public int RewardThreshold { get; set; } = 5000;
        public string RewardDescription { get; set; } = "Friday Pizza Night! 🍕";
        public string ResetSchedule { get; set; } = "Sunday 12:00 AM";
        public string InviteCode { get; private set; } = string.Empty;

        public event Action? OnChange;

        public ApartmentStateService(ApartmentClient apartmentClient, IJSRuntime js)
        {
            _apartmentClient = apartmentClient;
            _js = js;
        }

        public async Task LoadAsync(int? targetApartmentId = null)
        {
            int idToLoad = targetApartmentId ?? (Id > 0 ? Id : 1);

            try
            {
                var apt = await _apartmentClient.GetApartmentAsync(idToLoad);
                if (apt != null)
                {
                    Id = apt.Id;
                    ApartmentName = apt.Name;
                    RewardThreshold = apt.RewardThreshold;
                    ResetSchedule = string.IsNullOrWhiteSpace(apt.ResetSchedule) ? "Sunday 12:00 AM" : apt.ResetSchedule;
                    InviteCode = apt.InviteCode ?? string.Empty;
                }

                await LoadLocalSettingsAsync(idToLoad);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading apartment state: {ex.Message}");
            }
            finally
            {
                NotifyStateChanged();
            }
        }

        public async Task SaveAsync(string name, int threshold, string rewardDesc, string schedule)
        {
            try
            {
                ApartmentName = name;
                RewardThreshold = threshold;
                RewardDescription = rewardDesc;
                ResetSchedule = schedule;

                var updated = await _apartmentClient.UpdateApartmentAsync(new ApartmentDto
                {
                    Id = Id,
                    Name = name,
                    RewardThreshold = threshold,
                    ResetSchedule = schedule,
                    InviteCode = InviteCode
                });

                if (updated != null)
                {
                    ApartmentName = updated.Name;
                    RewardThreshold = updated.RewardThreshold;
                    ResetSchedule = updated.ResetSchedule;
                }

                await SaveLocalSettingsAsync(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving apartment state: {ex.Message}");
            }
            finally
            {
                NotifyStateChanged();
            }
        }

        
        public async Task DeleteApartmentAsync()
        {
            if (Id <= 0) return;

            int currentId = Id;
            try
            {
                var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7042/") };
                await httpClient.DeleteAsync($"api/Apartment/{currentId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting apartment from server: {ex.Message}");
                throw;
            }
            finally
            {
                try
                {
                    await _js.InvokeVoidAsync("localStorage.removeItem", $"choreboard_apt_extra_{currentId}");
                    await _js.InvokeVoidAsync("localStorage.removeItem", $"choreboard_columns_{currentId}");
                }
                catch { }
            }
        }

        private async Task SaveLocalSettingsAsync(int aptId)
        {
            if (aptId <= 0) return;
            try
            {
                var settings = new
                {
                    RewardDescription,
                    RewardThreshold,
                    ResetSchedule
                };
                var json = System.Text.Json.JsonSerializer.Serialize(settings);
                await _js.InvokeVoidAsync("localStorage.setItem", $"choreboard_apt_extra_{aptId}", json);
            }
            catch { }
        }

        private async Task LoadLocalSettingsAsync(int aptId)
        {
            if (aptId <= 0) return;
            try
            {
                var json = await _js.InvokeAsync<string>("localStorage.getItem", $"choreboard_apt_extra_{aptId}");
                if (!string.IsNullOrWhiteSpace(json))
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("RewardDescription", out var descElem))
                    {
                        RewardDescription = descElem.GetString() ?? RewardDescription;
                    }
                    if (doc.RootElement.TryGetProperty("RewardThreshold", out var threshElem))
                    {
                        if (threshElem.TryGetInt32(out var tVal)) RewardThreshold = tVal;
                    }
                    if (doc.RootElement.TryGetProperty("ResetSchedule", out var schedElem))
                    {
                        ResetSchedule = schedElem.GetString() ?? ResetSchedule;
                    }
                }
            }
            catch { }
        }

        public void Reset()
        {
            Id = 0;
            ApartmentName = "No room selected";
            RewardThreshold = 5000;
            RewardDescription = "Friday Pizza Night! 🍕";
            ResetSchedule = "Sunday 12:00 AM";
            InviteCode = string.Empty;
            NotifyStateChanged();
        }

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
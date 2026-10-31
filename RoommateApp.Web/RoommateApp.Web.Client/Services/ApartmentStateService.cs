using RoommateApp.Web.Client;

namespace RoommateApp.Web.Client.Services
{
    public class ApartmentStateService
    {
        private readonly ApartmentClient _apartmentClient;

        public int Id { get; private set; } = 1;
        public string ApartmentName { get; private set; } = "Apartment 4B";
        public int RewardThreshold { get; private set; } = 5000;
        public string ResetSchedule { get; private set; } = "Sunday 12:00 AM";

        public event Action? OnChange;

        public ApartmentStateService(ApartmentClient apartmentClient)
        {
            _apartmentClient = apartmentClient;
        }

        public async Task LoadAsync()
        {
            try
            {
                var apt = await _apartmentClient.GetApartmentAsync();
                if (apt != null)
                {
                    Id = apt.Id;
                    ApartmentName = apt.Name;
                    RewardThreshold = apt.RewardThreshold;
                    ResetSchedule = apt.ResetSchedule;
                    NotifyStateChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a szobaadatok betöltésekor: {ex.Message}");
            }
        }

        public async Task SaveAsync(string name, int threshold, string schedule)
        {
            var updated = await _apartmentClient.UpdateApartmentAsync(new ApartmentDto
            {
                Id = Id,
                Name = name,
                RewardThreshold = threshold,
                ResetSchedule = schedule
            });

            if (updated != null)
            {
                ApartmentName = updated.Name;
                RewardThreshold = updated.RewardThreshold;
                ResetSchedule = updated.ResetSchedule;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
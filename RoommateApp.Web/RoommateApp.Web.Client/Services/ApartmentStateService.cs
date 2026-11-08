using RoommateApp.Web.Client;

namespace RoommateApp.Web.Client.Services
{
    public class ApartmentStateService
    {
        private readonly ApartmentClient _apartmentClient;

        public int Id { get; private set; }
        public string ApartmentName { get; private set; } = "Nincs szoba kiválasztva";
        public int RewardThreshold { get; private set; } = 5000;
        public string ResetSchedule { get; private set; } = "Sunday 12:00 AM";
        public string InviteCode { get; private set; } = string.Empty;

        public event Action? OnChange;

        public ApartmentStateService(ApartmentClient apartmentClient)
        {
            _apartmentClient = apartmentClient;
        }

        // Betöltés a megadott targetApartmentId vagy a jelenlegi Id alapján
        public async Task LoadAsync(int? targetApartmentId = null)
        {
            int idToLoad = targetApartmentId ?? (Id > 0 ? Id : 1);

            try
            {
                // JAVÍTVA: Átadható az idToLoad a backend felé
                var apt = await _apartmentClient.GetApartmentAsync(idToLoad);
                if (apt != null)
                {
                    Id = apt.Id;
                    ApartmentName = apt.Name;
                    RewardThreshold = apt.RewardThreshold;
                    ResetSchedule = apt.ResetSchedule;
                    InviteCode = apt.InviteCode ?? string.Empty;
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
                ResetSchedule = schedule,
                InviteCode = InviteCode
            });

            if (updated != null)
            {
                ApartmentName = updated.Name;
                RewardThreshold = updated.RewardThreshold;
                ResetSchedule = updated.ResetSchedule;
                InviteCode = updated.InviteCode ?? InviteCode;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
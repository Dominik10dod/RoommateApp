using RoommateApp.Web.Client;

namespace RoommateApp.Web.Client.Services
{
    public class UserSessionService
    {
        public UserDto? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        public event Action? OnChange;
        public event Action? OnPointsChanged;

        public void SetUser(UserDto user)
        {
            CurrentUser = user;
            NotifyStateChanged();
        }

        public void Logout()
        {
            CurrentUser = null;
            NotifyStateChanged();
        }

        public void NotifyPointsChanged() => OnPointsChanged?.Invoke();

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
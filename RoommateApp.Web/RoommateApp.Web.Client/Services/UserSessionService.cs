using RoommateApp.Web.Client;

namespace RoommateApp.Web.Client.Services
{
    public class UserSessionService
    {
        public UserDto? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        public event Action? OnChange;

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

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
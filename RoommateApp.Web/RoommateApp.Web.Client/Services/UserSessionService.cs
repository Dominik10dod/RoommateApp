using System.Text.Json;
using Microsoft.JSInterop;
using RoommateApp.Web.Client;

namespace RoommateApp.Web.Client.Services
{
    public class UserSessionService
    {
        private const string StorageKey = "choreboard_current_user";
        private readonly IJSRuntime? _js;

        public UserDto? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        public event Action? OnChange;
        public event Action? OnPointsChanged;

        public UserSessionService(IJSRuntime? js = null)
        {
            _js = js;
        }

        public async Task InitializeAsync()
        {
            if (_js == null || IsLoggedIn) return;

            try
            {
                var json = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var savedUser = JsonSerializer.Deserialize<UserDto>(json);
                    if (savedUser != null && savedUser.Id > 0)
                    {
                        CurrentUser = savedUser;
                        NotifyStateChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restoring user session: {ex.Message}");
            }
        }

        public void SetUser(UserDto user)
        {
            CurrentUser = user;
            _ = SaveToStorageAsync(user);
            NotifyStateChanged();
        }

        public void Logout()
        {
            CurrentUser = null;
            _ = RemoveFromStorageAsync();
            NotifyStateChanged();
        }

        public void NotifyPointsChanged() => OnPointsChanged?.Invoke();

        private async Task SaveToStorageAsync(UserDto user)
        {
            if (_js == null) return;
            try
            {
                var json = JsonSerializer.Serialize(user);
                await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user session: {ex.Message}");
            }
        }

        private async Task RemoveFromStorageAsync()
        {
            if (_js == null) return;
            try
            {
                await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing user session: {ex.Message}");
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
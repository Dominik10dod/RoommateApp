using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace RoommateApp.Web.Client.Services
{
    public class RealtimeSyncService : IAsyncDisposable
    {
        private readonly NavigationManager _navigation;
        private HubConnection? _hubConnection;
        private int _currentApartmentId = 0;

        public event Action? OnBoardUpdated;

        public RealtimeSyncService(NavigationManager navigation)
        {
            _navigation = navigation;
        }

        public async Task JoinApartmentAsync(int apartmentId)
        {
            if (apartmentId <= 0) return;

            await EnsureConnectedAsync();

            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                if (_currentApartmentId > 0 && _currentApartmentId != apartmentId)
                {
                    await _hubConnection.InvokeAsync("LeaveApartmentGroup", _currentApartmentId);
                }

                if (_currentApartmentId != apartmentId)
                {
                    await _hubConnection.InvokeAsync("JoinApartmentGroup", apartmentId);
                    _currentApartmentId = apartmentId;
                }
            }
        }

        public async Task NotifyBoardUpdatedAsync(int apartmentId)
        {
            if (apartmentId <= 0) return;

            await EnsureConnectedAsync();

            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("NotifyBoardUpdated", apartmentId);
            }
        }

        private async Task EnsureConnectedAsync()
        {
            if (_hubConnection == null)
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(_navigation.ToAbsoluteUri("/chorehub"))
                    .WithAutomaticReconnect()
                    .Build();

                _hubConnection.On<int>("BoardUpdated", (apartmentId) =>
                {
                    if (apartmentId == _currentApartmentId)
                    {
                        OnBoardUpdated?.Invoke();
                    }
                });

                _hubConnection.Reconnected += async (_) =>
                {
                    if (_currentApartmentId > 0 && _hubConnection != null)
                    {
                        await _hubConnection.InvokeAsync("JoinApartmentGroup", _currentApartmentId);
                    }
                };
            }

            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _hubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SignalR connection error: {ex.Message}");
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
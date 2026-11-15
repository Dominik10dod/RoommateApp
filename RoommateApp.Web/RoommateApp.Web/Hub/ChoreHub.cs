using Microsoft.AspNetCore.SignalR;

namespace RoommateApp.Web.Hubs
{
    public class ChoreHub : Hub
    {
        public async Task JoinApartmentGroup(int apartmentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"apartment_{apartmentId}");
        }

        public async Task LeaveApartmentGroup(int apartmentId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"apartment_{apartmentId}");
        }

        public async Task NotifyBoardUpdated(int apartmentId)
        {
            await Clients.Group($"apartment_{apartmentId}").SendAsync("BoardUpdated", apartmentId);
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task RegisterUser(int userId)
        {
            // Lấy userId từ token JWT
            var tokenUserId = int.Parse(Context.User?.FindFirst("UserId")?.Value ?? "0");
            if (tokenUserId != userId)
            {
                throw new HubException("Unauthorized user.");
            }

            // Thêm người dùng vào nhóm dựa trên userId
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
            await Clients.Others.SendAsync("UserOnline", userId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = int.Parse(Context.User?.FindFirst("UserId")?.Value ?? "0");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
            await Clients.Others.SendAsync("UserOffline", userId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
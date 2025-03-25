using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<int, string> _userConnections = new();

        public async Task RegisterUser(int userId)
        {
            // Remove previous connection if exists
            if (_userConnections.TryGetValue(userId, out var oldConnectionId))
            {
                _userConnections.TryRemove(userId, out _);
                await Groups.RemoveFromGroupAsync(oldConnectionId, $"user-{userId}");
            }

            // Add new connection
            _userConnections[userId] = Context.ConnectionId;
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");

            // Notify others this user is online
            await Clients.Others.SendAsync("UserOnline", userId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != 0)
            {
                _userConnections.TryRemove(userId, out _);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");

                // Notify others this user is offline
                await Clients.Others.SendAsync("UserOffline", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
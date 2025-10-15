using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using ChatApp.API.Hubs;

namespace ChatApp.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public NotificationService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyNewMessageAsync(MessageDto message)
        {
            await _hubContext.Clients.Group(message.ChatRoomId.ToString())
                .SendAsync("ReceiveMessage", message);
        }

        public async Task NotifyUserJoinedAsync(Guid userId, Guid chatRoomId)
        {
            await _hubContext.Clients.Group(chatRoomId.ToString())
                .SendAsync("UserJoined", userId);
        }

        public async Task NotifyUserLeftAsync(Guid userId, Guid chatRoomId)
        {
            await _hubContext.Clients.Group(chatRoomId.ToString())
                .SendAsync("UserLeft", userId);
        }
    }
}

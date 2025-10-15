
using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;

namespace ChatApp.Application.Facades
{
    public class ChatFacade : IChatFacade
    {
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;

        public ChatFacade(IChatService chatService, INotificationService notificationService)
        {
            _chatService = chatService;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<MessageDto>> GetChatRoomMessagesAsync(Guid chatRoomId)
        {
            return await _chatService.GetChatRoomMessagesAsync(chatRoomId);
        }

        public async Task<MessageDto> SendMessageAsync(SendMessageDto messageDto)
        {
            var message = await _chatService.SendMessageAsync(messageDto);
            await _notificationService.NotifyNewMessageAsync(message);
            return message;
        }

        public async Task<bool> JoinChatRoomAsync(Guid userId, Guid chatRoomId)
        {
            var joined = await _chatService.JoinChatRoomAsync(userId, chatRoomId);
            if (joined)
                await _notificationService.NotifyUserJoinedAsync(userId, chatRoomId);
            return joined;
        }

        public async Task<bool> LeaveChatRoomAsync(Guid userId, Guid chatRoomId)
        {
            var left = await _chatService.LeaveChatRoomAsync(userId, chatRoomId);
            if (left)
                await _notificationService.NotifyUserLeftAsync(userId, chatRoomId);
            return left;
        }
    }
}
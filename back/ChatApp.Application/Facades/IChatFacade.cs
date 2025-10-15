using ChatApp.Application.DTOs;

namespace ChatApp.Application.Facades
{
    public interface IChatFacade
    {
        Task<IEnumerable<MessageDto>> GetChatRoomMessagesAsync(Guid chatRoomId);
        Task<MessageDto> SendMessageAsync(SendMessageDto messageDto);
        Task<bool> JoinChatRoomAsync(Guid userId, Guid chatRoomId);
        Task<bool> LeaveChatRoomAsync(Guid userId, Guid chatRoomId);
    }
}

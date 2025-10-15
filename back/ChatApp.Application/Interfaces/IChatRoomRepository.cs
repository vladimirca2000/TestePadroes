using ChatApp.Application.DTOs;
using ChatApp.Core.Entities;

namespace ChatApp.Application.Interfaces
{
    public interface IChatRoomRepository
    {
        Task<ChatRoom?> GetRoomWithUsersAsync(Guid roomId);
        Task<List<ChatRoom>> GetAllRoomsAsync();
        Task<User?> GetUserAsync(Guid userId);
        Task<bool> AddUserToRoomAsync(Guid userId, Guid roomId);
        Task<bool> RemoveUserFromRoomAsync(Guid userId, Guid roomId);
    }
}

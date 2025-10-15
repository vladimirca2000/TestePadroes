using ChatApp.Application.DTOs;
using System.Threading.Tasks;

namespace ChatApp.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyNewMessageAsync(MessageDto message);
        Task NotifyUserJoinedAsync(Guid userId, Guid chatRoomId);
        Task NotifyUserLeftAsync(Guid userId, Guid chatRoomId);
    }
}

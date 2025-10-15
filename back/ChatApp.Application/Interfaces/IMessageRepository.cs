using ChatApp.Application.DTOs;
using ChatApp.Core.Entities;

namespace ChatApp.Application.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message?> SendMessageAsync(SendMessageDto dto);
    }
}

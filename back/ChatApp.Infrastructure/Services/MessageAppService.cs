using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Services
{
    public class MessageAppService : IMessageAppService
    {
        private readonly ChatDbContext _db;
        public MessageAppService(ChatDbContext db)
        {
            _db = db;
        }

        public async Task<MessageDto?> SendMessageAsync(SendMessageDto dto)
        {
            var user = await _db.Users.FindAsync(dto.SenderId);
            var room = await _db.ChatRooms.FindAsync(dto.ChatRoomId);
            if (user == null || room == null)
                return null;
            var message = new ChatApp.Core.Entities.Message { Id = Guid.NewGuid(), ChatRoomId = room.Id, SenderId = user.Id, Content = dto.Content, SentAt = DateTime.UtcNow, IsRead = false };
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();
            return new MessageDto {
                Id = message.Id,
                Content = message.Content,
                SentAt = message.SentAt,
                SenderUsername = user.Username,
                ChatRoomId = room.Id,
                IsRead = false
            };
        }
    }
}

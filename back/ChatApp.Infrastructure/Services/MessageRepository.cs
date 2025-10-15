using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Core.Entities;
using ChatApp.Infrastructure.Data;

namespace ChatApp.Infrastructure.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ChatDbContext _db;
        public MessageRepository(ChatDbContext db)
        {
            _db = db;
        }

        public async Task<Message?> SendMessageAsync(SendMessageDto dto)
        {
            var user = await _db.Users.FindAsync(dto.SenderId);
            var room = await _db.ChatRooms.FindAsync(dto.ChatRoomId);
            if (user == null || room == null)
                return null;
            var message = new Message
            {
                Id = Guid.NewGuid(),
                ChatRoomId = room.Id,
                SenderId = user.Id,
                Content = dto.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();
            return message;
        }
    }
}

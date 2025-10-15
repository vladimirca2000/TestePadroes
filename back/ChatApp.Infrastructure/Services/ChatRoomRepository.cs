using ChatApp.Application.Interfaces;
using ChatApp.Core.Entities;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Services
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly ChatDbContext _db;
        public ChatRoomRepository(ChatDbContext db)
        {
            _db = db;
        }

        public async Task<ChatRoom?> GetRoomWithUsersAsync(Guid roomId)
        {
            return await _db.ChatRooms.Include(r => r.Users).FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<List<ChatRoom>> GetAllRoomsAsync()
        {
            return await _db.ChatRooms.ToListAsync();
        }

        public async Task<User?> GetUserAsync(Guid userId)
        {
            return await _db.Users.FindAsync(userId);
        }

        public async Task<bool> AddUserToRoomAsync(Guid userId, Guid roomId)
        {
            var user = await _db.Users.FindAsync(userId);
            var room = await _db.ChatRooms.Include(r => r.Users).FirstOrDefaultAsync(r => r.Id == roomId);
            if (user == null || room == null)
                return false;
            if (!room.Users.Any(u => u.Id == user.Id))
                room.Users.Add(user);
            else
                return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromRoomAsync(Guid userId, Guid roomId)
        {
            var user = await _db.Users.FindAsync(userId);
            var room = await _db.ChatRooms.Include(r => r.Users).FirstOrDefaultAsync(r => r.Id == roomId);
            if (user == null || room == null)
                return false;
            if (room.Users.Any(u => u.Id == user.Id))
            {
                room.Users.Remove(user);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

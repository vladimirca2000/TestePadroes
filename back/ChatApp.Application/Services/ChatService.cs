using System.Collections.Concurrent;
using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;

namespace ChatApp.Application.Services
{
    public class ChatService : IChatService
    {
        // Armazenamento em memória
        private static readonly ConcurrentDictionary<Guid, List<MessageDto>> _messages = new();
        private static readonly ConcurrentDictionary<Guid, HashSet<Guid>> _roomUsers = new();

        public Task<IEnumerable<MessageDto>> GetChatRoomMessagesAsync(Guid chatRoomId)
        {
            _messages.TryGetValue(chatRoomId, out var msgs);
            return Task.FromResult(msgs?.AsEnumerable() ?? Enumerable.Empty<MessageDto>());
        }

        public Task<MessageDto> SendMessageAsync(SendMessageDto messageDto)
        {
            var message = new MessageDto
            {
                Id = Guid.NewGuid(),
                Content = messageDto.Content,
                SentAt = DateTime.UtcNow,
                SenderUsername = $"User_{messageDto.SenderId.ToString().Substring(0, 8)}", // Simulação
                ChatRoomId = messageDto.ChatRoomId,
                IsRead = false
            };
            var list = _messages.GetOrAdd(messageDto.ChatRoomId, _ => new List<MessageDto>());
            lock (list) { list.Add(message); }
            return Task.FromResult(message);
        }

        public Task<bool> JoinChatRoomAsync(Guid userId, Guid chatRoomId)
        {
            var users = _roomUsers.GetOrAdd(chatRoomId, _ => new HashSet<Guid>());
            lock (users)
            {
                if (!users.Add(userId))
                    return Task.FromResult(false); // Já está na sala
            }
            return Task.FromResult(true);
        }

        public Task<bool> LeaveChatRoomAsync(Guid userId, Guid chatRoomId)
        {
            if (_roomUsers.TryGetValue(chatRoomId, out var users))
            {
                lock (users)
                {
                    if (users.Remove(userId))
                        return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }
    }
}

using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Facades;


namespace ChatApp.Application.Services
{
    public class ChatRoomAppService : IChatRoomAppService
    {
        private readonly IChatRoomRepository _repo;
        private readonly IChatFacade _chatFacade;
        public ChatRoomAppService(IChatRoomRepository repo, IChatFacade chatFacade)
        {
            _repo = repo;
            _chatFacade = chatFacade;
        }

        public async Task<IEnumerable<object>> GetRoomUsersAsync(Guid roomId)
        {
            var room = await _repo.GetRoomWithUsersAsync(roomId);
            if (room == null)
                return Enumerable.Empty<object>();
            return room.Users.Select(u => new { u.Id, u.Username, u.Email, u.CreatedAt, u.LastSeen });
        }

        public async Task<IEnumerable<object>> GetRoomsAsync()
        {
            var rooms = await _repo.GetAllRoomsAsync();
            return rooms.Select(r => new { r.Id, r.Name });
        }

        public async Task<(bool Success, string Message)> JoinRoomAsync(JoinRoomDto dto)
        {
            var user = await _repo.GetUserAsync(dto.UserId);
            var room = await _repo.GetRoomWithUsersAsync(dto.ChatRoomId);
            if (user == null || room == null)
                return (false, "Usuário ou sala não encontrada");
            var joined = await _chatFacade.JoinChatRoomAsync(dto.UserId, dto.ChatRoomId);
            if (!joined)
                return (false, "Usuário já está na sala");
            return (true, $"Usuário {user.Username} entrou na sala {room.Name}");
        }

        public async Task<(bool Success, string Message)> LeaveRoomAsync(JoinRoomDto dto)
        {
            var user = await _repo.GetUserAsync(dto.UserId);
            var room = await _repo.GetRoomWithUsersAsync(dto.ChatRoomId);
            if (user == null || room == null)
                return (false, "Usuário ou sala não encontrada");
            var left = await _chatFacade.LeaveChatRoomAsync(dto.UserId, dto.ChatRoomId);
            if (left)
                return (true, $"Usuário {user.Username} saiu da sala {room.Name}");
            return (false, "Usuário não está na sala");
        }

        // Novo método: obter mensagens da sala via facade
        public async Task<IEnumerable<MessageDto>> GetRoomMessagesAsync(Guid roomId)
        {
            return await _chatFacade.GetChatRoomMessagesAsync(roomId);
        }

        // Novo método: enviar mensagem via facade
        public async Task<MessageDto> SendMessageAsync(SendMessageDto dto)
        {
            return await _chatFacade.SendMessageAsync(dto);
        }
    }
}

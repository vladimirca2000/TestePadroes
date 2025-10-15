

using ChatApp.Application.DTOs;

namespace ChatApp.Application.Interfaces
{
	public interface IChatRoomAppService
	{
		Task<IEnumerable<object>> GetRoomUsersAsync(Guid roomId);
		Task<IEnumerable<object>> GetRoomsAsync();
		Task<(bool Success, string Message)> JoinRoomAsync(JoinRoomDto dto);
		Task<(bool Success, string Message)> LeaveRoomAsync(JoinRoomDto dto);
	}
}


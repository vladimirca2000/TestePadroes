

using ChatApp.Application.DTOs;

namespace ChatApp.Application.Interfaces
{
	public interface IMessageAppService
	{
		Task<MessageDto?> SendMessageAsync(SendMessageDto dto);
	}
}


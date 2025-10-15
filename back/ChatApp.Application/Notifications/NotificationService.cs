using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChatApp.Application.Notifications
{
	public class NotificationService : INotificationService
	{
		private readonly ILogger<NotificationService> _logger;

		public NotificationService(ILogger<NotificationService> logger)
		{
			_logger = logger;
		}

		public Task NotifyNewMessageAsync(MessageDto message)
		{
			_logger.LogInformation("Nova mensagem na sala {ChatRoomId} de {Sender}: {Content}", message.ChatRoomId, message.SenderUsername, message.Content);
			return Task.CompletedTask;
		}

		public Task NotifyUserJoinedAsync(Guid userId, Guid chatRoomId)
		{
			_logger.LogInformation("Usuário {UserId} entrou na sala {ChatRoomId}", userId, chatRoomId);
			return Task.CompletedTask;
		}

		public Task NotifyUserLeftAsync(Guid userId, Guid chatRoomId)
		{
			_logger.LogInformation("Usuário {UserId} saiu da sala {ChatRoomId}", userId, chatRoomId);
			return Task.CompletedTask;
		}
	}
}



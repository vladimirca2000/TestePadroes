using ChatApp.Application.DTOs;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Facades;


namespace ChatApp.Application.Services
{
    public class MessageAppService : IMessageAppService
    {
        private readonly IChatFacade _chatFacade;
        public MessageAppService(IChatFacade chatFacade)
        {
            _chatFacade = chatFacade;
        }

        public async Task<MessageDto?> SendMessageAsync(SendMessageDto dto)
        {
            // Usa o facade para enviar mensagem, centralizando regras e notificações
            return await _chatFacade.SendMessageAsync(dto);
        }
    }
}

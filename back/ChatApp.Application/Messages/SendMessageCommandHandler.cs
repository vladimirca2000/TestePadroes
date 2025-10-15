using MediatR;
using ChatApp.Application.DTOs;
using ChatApp.Application.Facades;

namespace ChatApp.Application.Messages
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, MessageDto>
    {
        private readonly IChatFacade _chatFacade;

        public SendMessageCommandHandler(IChatFacade chatFacade)
        {
            _chatFacade = chatFacade;
        }

        public async Task<MessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            return await _chatFacade.SendMessageAsync(request.Dto);
        }
    }
}

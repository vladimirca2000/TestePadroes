using MediatR;
using ChatApp.Application.DTOs;

namespace ChatApp.Application.Messages
{
    public record SendMessageCommand(SendMessageDto Dto) : IRequest<MessageDto>;
}

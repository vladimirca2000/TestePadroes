using Microsoft.AspNetCore.SignalR;
using ChatApp.Application.Facades;
using ChatApp.Application.DTOs;
using MediatR;
using ChatApp.Application.Messages;
using ChatApp.API.Services;

namespace ChatApp.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly ChatApp.API.Services.ChatBotService _chatBotService;

        public ChatHub(IMediator mediator, ChatBotService chatBotService)
        {
            _mediator = mediator;
            _chatBotService = chatBotService;
        }

        public async Task JoinChatRoom(string chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
            await Clients.Group(chatRoomId).SendAsync("UserJoined", Context.UserIdentifier);
        }

        public async Task LeaveChatRoom(string chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
            await Clients.Group(chatRoomId).SendAsync("UserLeft", Context.UserIdentifier);
        }


        public async Task SendMessage(string chatRoomId, string user, string message)
        {
            // Envia a mensagem via Mediator
            var dto = new SendMessageDto { ChatRoomId = Guid.Parse(chatRoomId), SenderId = Guid.Parse(user), Content = message };
            var result = await _mediator.Send(new SendMessageCommand(dto));

            await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", new { User = user, Message = message, Bot = false });

            // Processa resposta do bot
            var botResponse = await _chatBotService.ProcessMessageAsync(message);
            if (!string.IsNullOrEmpty(botResponse))
            {
                await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", new { User = "ChatBot", Message = botResponse, Bot = true });
            }
        }

        public override async Task OnConnectedAsync()
        {
            // Atualizar status do usuário para online
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Atualizar status do usuário para offline
            await base.OnDisconnectedAsync(exception);
        }
    }

    
}

using ChatApp.Application.Interfaces;
using ChatApp.Application.Facades;
using ChatApp.Application.DTOs;
using ChatApp.Core.Entities;
using ChatApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchangeRedis = StackExchange.Redis;

namespace ChatApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatRoomAppService _chatRoomAppService;
        private readonly IMessageAppService _messageAppService;
        private readonly StackExchange.Redis.IConnectionMultiplexer _redis;
        public ChatController(IChatRoomAppService chatRoomAppService, IMessageAppService messageAppService, StackExchange.Redis.IConnectionMultiplexer redis)
        {
            _chatRoomAppService = chatRoomAppService;
            _messageAppService = messageAppService;
            _redis = redis;
        }

        [HttpGet("room-users/{roomId}")]
        public async Task<IActionResult> GetRoomUsers(Guid roomId)
        {
            var users = await _chatRoomAppService.GetRoomUsersAsync(roomId);
            if (!users.Any())
                return NotFound("Sala não encontrada");
            return Ok(users);
        }

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _chatRoomAppService.GetRoomsAsync();
            return Ok(rooms);
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinRoom([FromBody] JoinRoomDto dto)
        {
            (bool success, string message) = await _chatRoomAppService.JoinRoomAsync(dto);
            if (!success)
                return BadRequest(message);
            return Ok(new { Message = message });
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
        {
            var messageDto = await _messageAppService.SendMessageAsync(dto);
            if (messageDto == null)
                return NotFound("Usuário ou sala não encontrada");
            // Salvar mensagem no Redis
            var db = _redis.GetDatabase();
            var redisKey = $"chat:room:{messageDto.ChatRoomId}:messages";
            var serialized = System.Text.Json.JsonSerializer.Serialize(new {
                messageDto.Id,
                messageDto.Content,
                messageDto.SentAt,
                messageDto.SenderUsername,
                messageDto.ChatRoomId,
                messageDto.IsRead
            });
            await db.ListRightPushAsync(redisKey, serialized);
            return Ok(messageDto);
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveRoom([FromBody] JoinRoomDto dto)
        {
            (bool success, string message) = await _chatRoomAppService.LeaveRoomAsync(dto);
            if (!success)
                return BadRequest(message);
            return Ok(new { Message = message });
        }
    }
}

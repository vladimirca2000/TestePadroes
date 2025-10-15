using System;
using System.Threading.Tasks;

namespace ChatApp.API.Services
{
    public class ChatBotService
    {
        public Task<string?> ProcessMessageAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return Task.FromResult<string?>(null);

            if (message.StartsWith("/ajuda", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult<string?>("Comandos disponíveis: /ajuda, /hora, /ping");

            if (message.StartsWith("/hora", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult<string?>("Agora são: " + DateTime.Now.ToString("HH:mm:ss"));

            if (message.StartsWith("/ping", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult<string?>("pong");

            // Pode adicionar mais comandos aqui

            return Task.FromResult<string?>(null);
        }
    }
}

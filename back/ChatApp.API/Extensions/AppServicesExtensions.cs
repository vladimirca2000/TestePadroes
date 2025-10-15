using MediatR;
using ChatApp.Application.Messages; // Para referência do assembly

namespace ChatApp.API.Extensions
{
    public static class AppServicesExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ChatApp.Application.Interfaces.IMessageRepository, ChatApp.Infrastructure.Services.MessageRepository>();
            services.AddScoped<ChatApp.Application.Interfaces.IChatRoomRepository, ChatApp.Infrastructure.Services.ChatRoomRepository>();
            services.AddScoped<ChatApp.Application.Interfaces.IChatRoomAppService, ChatApp.Application.Services.ChatRoomAppService>();
            services.AddScoped<ChatApp.Application.Interfaces.IMessageAppService, ChatApp.Application.Services.MessageAppService>();
            services.AddScoped<ChatApp.Application.Interfaces.INotificationService, ChatApp.Application.Notifications.NotificationService>();
            services.AddScoped<ChatApp.Application.Interfaces.IChatService, ChatApp.Application.Services.ChatService>();
            services.AddScoped<ChatApp.Application.Facades.IChatFacade, ChatApp.Application.Facades.ChatFacade>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SendMessageCommand>());
            return services;
        }
    }
}

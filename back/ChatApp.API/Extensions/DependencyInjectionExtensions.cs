using ChatApp.API.Services;
using ChatApp.Application.Facades;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Services;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ChatApp.API.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Application, Infrastructure, Notifications
            services.AddScoped<ChatApp.Application.Interfaces.IMessageRepository, ChatApp.Infrastructure.Services.MessageRepository>();
            services.AddScoped<ChatApp.Application.Interfaces.IChatRoomRepository, ChatApp.Infrastructure.Services.ChatRoomRepository>();
            services.AddScoped<ChatApp.Application.Interfaces.IChatRoomAppService, ChatApp.Application.Services.ChatRoomAppService>();
            services.AddScoped<ChatApp.Application.Interfaces.IMessageAppService, ChatApp.Application.Services.MessageAppService>();
            services.AddScoped<ChatApp.Application.Interfaces.INotificationService, ChatApp.Application.Notifications.NotificationService>();
            services.AddScoped<IChatFacade, ChatFacade>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<INotificationService, NotificationService>();

            // Entity Framework Core (MySQL)
            services.AddDbContext<ChatDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(10, 6, 0))
                ));

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(
                    configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis connection string is missing.")
                )
            );

            // SignalR (com Redis backplane)
            services.AddSignalR()
                .AddStackExchangeRedis(configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis connection string is missing for SignalR."));

            // Swagger/OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // CORS (para Angular)
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Controllers
            services.AddControllers();

            return services;
        }
    }
}

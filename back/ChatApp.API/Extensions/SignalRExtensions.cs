namespace ChatApp.API.Extensions
{
    public static class SignalRExtensions
    {
        public static IServiceCollection AddSignalRWithRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR()
                .AddStackExchangeRedis(configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis connection string is missing for SignalR."));
            return services;
        }
    }
}

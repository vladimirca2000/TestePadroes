using StackExchange.Redis;

namespace ChatApp.API.Extensions
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(
                    configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis connection string is missing.")
                )
            );
            return services;
        }
    }
}

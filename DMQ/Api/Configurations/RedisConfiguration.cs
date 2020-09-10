using Api.Subscribers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Api.Configurations
{
    public static class RedisConfiguration
    {
        public static void AddRedisMessageQueue(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(x =>
                ConnectionMultiplexer.Connect(configuration.GetValue<string>("RedisConnection"))
            );

            services.AddHostedService<RedisSubscriberOne>();
            services.AddHostedService<RedisSubscriberTwo>();
        }
    }
}
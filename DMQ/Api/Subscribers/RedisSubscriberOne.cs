using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Api.Subscribers
{
    public class RedisSubscriberOne : BackgroundService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisSubscriberOne(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _connectionMultiplexer.GetSubscriber();
            
            return subscriber.SubscribeAsync("messages", (channel, value) =>
            {
                Console.WriteLine($"1 - {channel}: {value}");
            });
        }
    }
}

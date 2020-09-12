using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Interfaces;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Api.Subscribers
{
    public class RedisSubscriberTwo : BackgroundService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ISomeService _someService;

        public RedisSubscriberTwo(IConnectionMultiplexer connectionMultiplexer, ISomeService someService)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _someService = someService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _connectionMultiplexer.GetSubscriber();
            
            return subscriber.SubscribeAsync("messages", async (channel, value) =>
            {
                Console.WriteLine($"2 - {channel}: {value}");
                await _someService.DoSomethingAsync();
            });
        }
    }
}

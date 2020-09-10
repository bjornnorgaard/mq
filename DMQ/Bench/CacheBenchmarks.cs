using Api.Dto;
using Api.Services;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using MemoryCache = Api.Services.MemoryCache;

namespace Bench
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CacheBenchmarks
    {
        private readonly Person _person;
        private readonly string _key;
        private readonly MemoryCache _memoryCache;
        private readonly RedisCache _redisCache;

        public CacheBenchmarks()
        {
            var provider = new ServiceCollection()
                .AddMemoryCache()
                .AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect("localhost:6379,localhost:6380"))
                .BuildServiceProvider();

            var iMemoryCache = provider.GetService<IMemoryCache>();
            _memoryCache = new MemoryCache(iMemoryCache);

            var multiplexer = provider.GetService<IConnectionMultiplexer>();
            _redisCache = new RedisCache(multiplexer);

            _key = "oij";
            _person = new Person {Id = 1, Name = "John Doe", Age = 22};
        }

        [Benchmark]
        public void MemoryCache()
        {
            _memoryCache.SetAsync(_key, _person);
            _memoryCache.GetAsync<Person>(_key);
        }

        [Benchmark]
        public void RedisCache()
        {
            _redisCache.SetAsync(_key, _person);
            _redisCache.GetAsync<Person>(_key);
        }
    }
}
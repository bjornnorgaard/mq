using Api.Database;
using Api.Dto;
using Api.Services;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.EntityFrameworkCore;
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
        private readonly DatabaseCache _databaseCache;

        public CacheBenchmarks()
        {
            var provider = new ServiceCollection()
                .AddMemoryCache()
                .AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect("localhost:6379,localhost:6380"))
                .AddDbContext<Context>(o => o.UseSqlServer("Server=localhost;Database=master;User=sa;Password=Your_password123;"))
                .BuildServiceProvider();

            var iMemoryCache = provider.GetService<IMemoryCache>();
            _memoryCache = new MemoryCache(iMemoryCache);

            var multiplexer = provider.GetService<IConnectionMultiplexer>();
            _redisCache = new RedisCache(multiplexer);

            var context = provider.GetService<Context>();
            _databaseCache = new DatabaseCache(context);

            _key = "oij";
            _person = new Person {Id = 1, Name = "John Doe", Age = 22};
        }

        [Benchmark(Baseline = true)]
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

        [Benchmark]
        public void DatabaseCache()
        {
            _databaseCache.SetAsync(_key, _person);
            _databaseCache.GetAsync<Person>(_key);
        }
    }
}

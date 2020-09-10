using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Api.Services
{
    public class RedisCache : ICache
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCache(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var db = _redis.GetDatabase();
            
            var value = await db.StringGetAsync(key);
            
            if (!value.HasValue) throw new ArgumentException("Key not found");

            var result = JsonConvert.DeserializeObject<T>(value);

            return result;
        }

        public async Task SetAsync(string key, object value)
        {
            var db = _redis.GetDatabase();

            var json = JsonConvert.SerializeObject(value);

            await db.StringSetAsync(key, json);
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Api.Services
{
    public class MemoryCache : ICache
    {
        private readonly IMemoryCache _cache;

        public MemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T> GetAsync<T>(string key)
        {
            var found = _cache.TryGetValue<string>(key, out var json);

            if (!found) throw new ArgumentException("Key not found");
            
            var value = JsonConvert.DeserializeObject<T>(json);

            return Task.FromResult(value);
        }

        public Task SetAsync(string key, object value)
        {
            var json = JsonConvert.SerializeObject(value);

            _cache.Set(key, json);
            
            return Task.CompletedTask;
        }
    }
}
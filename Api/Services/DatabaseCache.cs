using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Database;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api.Services
{
    public class DatabaseCache : ICache
    {
        private readonly Context _context;

        public DatabaseCache(Context context)
        {
            _context = context;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var json = await _context.CachedItems
                .Where(ci => ci.Key == key)
                .Select(ci => ci.Value)
                .FirstOrDefaultAsync();

            if (json == null) throw new ArgumentException("Key not found");
            
            var obj = JsonConvert.DeserializeObject<T>(json);

            return obj;
        }

        public async Task SetAsync(string key, object value)
        {
            var existingItem = await _context.CachedItems
                .FirstOrDefaultAsync(ci => ci.Key == key);

            var json = JsonConvert.SerializeObject(value);

            if (existingItem != null)
            {
                existingItem.Value = json;
            }
            else
            {
                var newCachedItem = new CachedItem {Key = key, Value = json};
                await _context.AddAsync(newCachedItem);
            }

            await _context.SaveChangesAsync();
        }
    }
}
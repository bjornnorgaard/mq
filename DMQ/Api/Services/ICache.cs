using System.Threading.Tasks;

namespace Api.Services
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object value);
    }
}
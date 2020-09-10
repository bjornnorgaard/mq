using System.Threading.Tasks;
using Api.Dto;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Api.Controllers
{
    [Route("redis"), ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ICache _cache;

        public RedisController(IConnectionMultiplexer redis, ICache cache)
        {
            _redis = redis;
            _cache = cache;
        }

        [HttpPost("send-message")]
        public ActionResult SendMessage([FromQuery] string message)
        {
            var subscriber =  _redis.GetSubscriber();
            subscriber.PublishAsync("messages", message);
            return Ok(message);
        }

        [HttpPost("{key}/get")]
        public async Task<ActionResult<Person>> Get([FromRoute] string key)
        {
            var value = await _cache.GetAsync<Person>(key);
            return Ok(value);
        }

        [HttpPost("{key}/set")]
        public async Task<ActionResult> Set([FromRoute] string key, [FromBody] Person value)
        {
            await _cache.SetAsync(key, value);
            return Ok();
        }
    }
}

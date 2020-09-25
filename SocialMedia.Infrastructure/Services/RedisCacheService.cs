using StackExchange.Redis;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionRedis;

        public RedisCacheService(IConnectionMultiplexer connectionRedis)
        {
            _connectionRedis = connectionRedis;
        }

        public async Task<string> GetCaheValueAsync(string key)
        {
            var db = _connectionRedis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task SetCacheValueAsync(string key, string value) 
        {
            var db = _connectionRedis.GetDatabase();
            await db.StringSetAsync(key, value);
        }
    }
}

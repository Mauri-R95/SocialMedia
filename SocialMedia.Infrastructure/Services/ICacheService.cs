using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Services
{
    public interface ICacheService
    {
        Task<string> GetCaheValueAsync(string key);
        Task SetCacheValueAsync(string key, string value);

    }
}
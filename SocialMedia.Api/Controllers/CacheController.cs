using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Application.Model;
using SocialMedia.Infrastructure.Services;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        public readonly ICacheService _cacheService;
        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetCacheValue([FromRoute] string key) 
        {
            var value = await _cacheService.GetCaheValueAsync(key);
            return string.IsNullOrEmpty(value) ? (IActionResult)NotFound() : Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> SetCacheValue([FromBody] CacheDto cache)
        {
            await _cacheService.SetCacheValueAsync(cache.Key,cache.Value);
            return Ok();
        }
    }
}

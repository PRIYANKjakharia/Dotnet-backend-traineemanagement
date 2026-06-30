using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TraineeManagement.API.Interfaces;
 
namespace TraineeManagement.API.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<TraineeService> _logger;
        public RedisCacheService(IDistributedCache cache , ILogger<TraineeService> logger)
        {
            _cache = cache;
            _logger = logger;
        }
 
        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var data = await _cache.GetStringAsync(key);
                if (string.IsNullOrEmpty(data)) return default;
                    
                return JsonSerializer.Deserialize<T>(data);
            }
            catch
            {
                return default;
            }
 
        }
 
        public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry
                };
    
                var jsonData = JsonSerializer.Serialize(value);
    
                await _cache.SetStringAsync(key, jsonData, options);
            }
            catch
            {
                _logger.LogError("Redis is inactive");   
            }
        }
 
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
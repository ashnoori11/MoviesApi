using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services.CacheService.CacheDecorators;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal class DistributedCacheDecorator : CacheManager
{
    public DistributedCacheDecorator(IMemoryCache memoryCache, IDistributedCache distributedCache)
        : base(memoryCache, distributedCache) { }

    public override async Task<T> GetAsync<T>(string key,CancellationToken cancellationToken)
    {
        var bytes = await _distributedCache.GetAsync(key,cancellationToken);

        if (bytes != null)
            return JsonSerializer.Deserialize<T>(bytes);

        return default;
    }

    public override async Task SetAsync<T>(string key, T value,CancellationToken cancellationToken, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions();

        if (expiration.HasValue)
            options.SetAbsoluteExpiration(expiration.Value);

        await _distributedCache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), options,cancellationToken);
    }

    public override async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(key,cancellationToken);
    }

    public override async Task<bool> ContainsAsync(string key,CancellationToken cancellationToken)
    {
        return _distributedCache.GetAsync(key, cancellationToken) != null;
    }


    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}

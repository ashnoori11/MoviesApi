using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace Application.Services.CacheService.CacheDecorators;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal class InMemoryCacheDecorator : CacheManager
{
    public InMemoryCacheDecorator(IMemoryCache memoryCache, IDistributedCache distributedCache) :
        base(memoryCache, distributedCache)
    { }

    public override T Get<T>(string key)
        => _memoryCache.Get<T>(key);

    public override void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        if (expiration.HasValue)
            _memoryCache.Set(key, value, expiration.Value);
        else
            _memoryCache.Set(key, value);
    }

    public override void Remove(string key)
        => _memoryCache.Remove(key);

    public override bool Contains(string key)
        => _memoryCache.TryGetValue(key, out _);

    private string? GetDebuggerDisplay() => ToString();
}

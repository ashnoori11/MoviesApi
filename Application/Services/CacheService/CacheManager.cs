using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services.CacheService;

public class CacheManager(IMemoryCache memoryCache, IDistributedCache distributedCache) : ICacheManager
{
    protected readonly IMemoryCache _memoryCache = memoryCache;
    protected readonly IDistributedCache _distributedCache = distributedCache;

    public virtual bool Contains(string key)
    {
        throw new NotImplementedException();
    }

    public virtual Task<bool> ContainsAsync(string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public virtual T Get<T>(string key)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public virtual void Remove(string key)
    {
        throw new NotImplementedException();
    }

    public virtual Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public virtual void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        throw new NotImplementedException();
    }

    public virtual Task SetAsync<T>(string key, T value, CancellationToken cancellationToken, TimeSpan? expiration = null)
    {
        throw new NotImplementedException();
    }
}

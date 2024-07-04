using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services.CacheService.CacheDecorators;

public class CompositeCacheManagerDecorator : ICacheManager
{
    private readonly InMemoryCacheDecorator _inMemoryDecorator;
    private readonly DistributedCacheDecorator _distributedDecorator;

    public CompositeCacheManagerDecorator(
        IMemoryCache memoryCache,
        IDistributedCache distributedCache)
    {
        _inMemoryDecorator = new InMemoryCacheDecorator(memoryCache, distributedCache);
        _distributedDecorator = new DistributedCacheDecorator(memoryCache, distributedCache);
    }

    /// <summary>
    /// only get data from in memory cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T Get<T>(string key)
    {
        return _inMemoryDecorator.Get<T>(key);
    }

    /// <summary>
    /// only add data to in memory cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiration"></param>
    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        _inMemoryDecorator.Set(key, value, expiration);
    }

    /// <summary>
    /// remove data from in memory cach by its key
    /// </summary>
    /// <param name="key"></param>
    public void Remove(string key)
    {
        _inMemoryDecorator.Remove(key);
        _distributedDecorator.Remove(key);
    }

    /// <summary>
    /// only search for key through in memory cache
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Contains(string key)
    {
        return _inMemoryDecorator.Contains(key);
    }

    /// <summary>
    /// only get data from disterbuted cache if can not find then looks for key in InMemory cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        T value = await _distributedDecorator.GetAsync<T>(key, cancellationToken);
        if (value == null)
        {
            value = _inMemoryDecorator.Get<T>(key);
        }

        return value;
    }


    /// <summary>
    /// add data in both : disterbuted cache and in memory cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="expiration"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken, TimeSpan? expiration = null)
    {
        await _distributedDecorator.SetAsync<T>(key, value, cancellationToken, expiration);
        _inMemoryDecorator.Set<T>(key, value,expiration);
    }


    /// <summary>
    /// remove data in both : distebuted cach and in memory cache by the same key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await _distributedDecorator.RemoveAsync(key,cancellationToken);
        _inMemoryDecorator.Remove(key);
    }


    /// <summary>
    /// search for key through both : in memory cache and disterbuted cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<bool> ContainsAsync(string key, CancellationToken cancellationToken)
    {
        bool isExists = await _distributedDecorator.ContainsAsync(key,cancellationToken);
        if (!isExists)
            isExists = _inMemoryDecorator.Contains(key);

        return isExists;
    }
}
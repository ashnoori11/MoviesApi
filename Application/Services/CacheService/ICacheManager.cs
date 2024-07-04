namespace Application.Services.CacheService;

public interface ICacheManager
{
    T Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan? expiration = null);
    void Remove(string key);
    bool Contains(string key);

    Task<T> GetAsync<T>(string key,CancellationToken cancellationToken);
    Task SetAsync<T>(string key, T value,CancellationToken cancellationToken, TimeSpan? expiration = null);
    Task RemoveAsync(string key, CancellationToken cancellationToken);
    Task<bool> ContainsAsync(string key, CancellationToken cancellationToken);
}

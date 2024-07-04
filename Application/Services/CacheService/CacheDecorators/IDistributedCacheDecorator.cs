
namespace Application.Services.CacheService.CacheDecorators
{
    public interface IDistributedCacheDecorator
    {
        Task<bool> ContainsAsync(string key, CancellationToken cancellationToken);
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken);
        Task RemoveAsync(string key, CancellationToken cancellationToken);
        Task SetAsync<T>(string key, T value, CancellationToken cancellationToken, TimeSpan? expiration = null);
    }
}
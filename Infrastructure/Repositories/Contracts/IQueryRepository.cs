using System.Linq.Expressions;

namespace Infrastructure.Repositories.Contracts;

public delegate IQueryable<T> FilterFunction<T>(IQueryable<T> query);
public interface IQueryRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllRows(int page, int pageSize, CancellationToken cancellationToken, string orderBy = null, string orderDirection = null);
    Task<IEnumerable<T>> QueryWithFilters(FilterFunction<T> filters,CancellationToken cancellationToken);

    Task<(IEnumerable<T> DataList, int TotalCount)> GetAllRowsNoTracking(int page, int pageSize, CancellationToken cancellationToken, string orderBy = null, string orderDirection = null);

    //Task<IEnumerable<T>> GetAllRowsNoTracking(int page, int pageSize, CancellationToken cancellationToken, string orderBy = null, string orderDirection = null);
    Task<IEnumerable<T>> QueryWithFiltersNoTracking(FilterFunction<T> filters, CancellationToken cancellationToken);
}
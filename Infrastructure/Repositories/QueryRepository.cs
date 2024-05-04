using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QueryRepository<T> : IQueryRepository<T> where T : class
{
    protected MoviesContext _dbContext;
    private DbSet<T> _dbSet;

    public QueryRepository(MoviesContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllRows(int page, int pageSize,CancellationToken cancellationToken, string orderBy = null, string orderDirection = null)
    {
        IQueryable<T> query = _dbSet;

        if (!string.IsNullOrEmpty(orderBy))
            query = orderDirection == "asc" ? query.OrderBy(t => t) : query.OrderByDescending(t => t);

        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> QueryWithFilters(FilterFunction<T> filters,CancellationToken cancellationToken)
    {
        IQueryable<T> query = _dbSet;

        query = filters(query);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<T> DataList,int TotalCount)> GetAllRowsNoTracking(int page, int pageSize, CancellationToken cancellationToken, string orderBy = null, string orderDirection = null)
    {
        IQueryable<T> query = _dbSet;

        if (!string.IsNullOrEmpty(orderBy))
            query = orderDirection == "asc" ? query.OrderBy(t => t) : query.OrderByDescending(t => t);

        var allRowsCount = await query.CountAsync(cancellationToken);
        var dataList = await query.AsNoTracking().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return (dataList,allRowsCount);
    }

    public async Task<IEnumerable<T>> QueryWithFiltersNoTracking(FilterFunction<T> filters, CancellationToken cancellationToken)
    {
        IQueryable<T> query = _dbSet;

        query = filters(query);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
}

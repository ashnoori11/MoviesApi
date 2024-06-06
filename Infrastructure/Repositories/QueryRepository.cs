using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
    public async Task<IEnumerable<TOutput>> GetSpecificColumsAsync<TOutput>(Func<T, TOutput> selector,CancellationToken cancellationToken)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        var result = query.Select(selector);

        cancellationToken.ThrowIfCancellationRequested();

        return result.ToList();
    }

    public async Task<IEnumerable<TOutput>> GetSpecificColumsByFilterAsync<TOutput>(Expression<Func<T,bool>> whereCluses,Func<T, TOutput> selector, CancellationToken cancellationToken)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        query = query.Where(whereCluses);
        var result = query.Select(selector);

        cancellationToken.ThrowIfCancellationRequested();
        return result.ToList();
    }

    public async Task<IEnumerable<TOutput>> GetSpecificColumsByFilterAndOrderByAsync<TOutput>(
    Expression<Func<T, bool>> whereCluses,
    Func<T, TOutput> selector,
    CancellationToken cancellationToken,
    Func<IQueryable, IOrderedQueryable<T>> orderBy = null)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        query = query.Where(whereCluses);

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var result = query.Select(selector);

        cancellationToken.ThrowIfCancellationRequested();
        return result.ToList();
    }

    public async Task<IEnumerable<TOutput>> GetSpecificColumsByFilterAndOrderByWithJoinsAsync<TOutput>(Expression<Func<T, bool>> whereCluses,
        Func<T, TOutput> selector,
        Func<IQueryable, IOrderedQueryable<T>> orderBy,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        query = query.Where(whereCluses);

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var result = query.Select(selector);

        cancellationToken.ThrowIfCancellationRequested();
        return result.ToList();
    }
}

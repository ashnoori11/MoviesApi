using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWorks;

public class UnitOfwork : IDisposable, IUnitOfWork
{
    public MoviesContext _context { get; }

    public UnitOfwork(MoviesContext context)
    {
        _context = context;
    }

    #region query repository
    public IQueryRepository<T> GetQueryRepository<T>() where T : class
        => new QueryRepository<T>(_context);
    #endregion

    #region genre repository
    private IGenresRepository _genreRepository;
    public IGenresRepository GenreRepository
    {
        get
        {
            if (_genreRepository is null)
                _genreRepository = new GenresRepository(_context);

            return _genreRepository;
        }
    }
    #endregion

    #region actor repository
    private IActorRepository _actorRepository;
    public IActorRepository ActorRepository
    {
        get
        {
            if (_actorRepository is null)
                _actorRepository = new ActorRepository(_context);

            return _actorRepository;
        }
    }
    #endregion


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw;
        }
    }
    public void Dispose() => _context.Dispose();
}

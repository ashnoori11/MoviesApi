using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;

namespace Infrastructure.UnitOfWorks;

public interface IUnitOfWork
{
    public MoviesContext _context { get; }
    IGenresRepository GenreRepository { get; }
    IActorRepository ActorRepository { get; }
    IQueryRepository<T> GetQueryRepository<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

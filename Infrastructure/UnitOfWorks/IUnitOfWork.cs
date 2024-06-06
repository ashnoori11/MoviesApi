using Infrastructure.Common.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;

namespace Infrastructure.UnitOfWorks;

public interface IUnitOfWork
{
    public MoviesContext _context { get; }
    IGenresRepository GenreRepository { get; }
    IActorRepository ActorRepository { get; }
    IQueryRepository<T> GetQueryRepository<T>() where T : class;
    IMovieTheaterRepository MovieTheaterRepository { get; }
    IMovieRepository MovieRepository { get; }

    Task<SaveChangeStatus> SaveChangesAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync<T>(CancellationToken cancellationToken);
}

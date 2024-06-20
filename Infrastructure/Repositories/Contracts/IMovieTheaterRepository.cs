using Domain.Entities;

namespace Infrastructure.Repositories.Contracts;

public interface IMovieTheaterRepository
{
    Task<Domain.Entities.MovieTheater?> GetMovieTheaterByIdAsNoTracking(int id, CancellationToken cancellationToken);
    Task<Domain.Entities.MovieTheater?> GetMovieTheaterById(int id, CancellationToken cancellationToken);

    Task<List<MovieTheater>> GetMovieTheatersByIdAsNoTracking(int[] ids, CancellationToken cancellationToken);
    Task InsertMovieTheaterAsync(MovieTheater movieTheater, CancellationToken cancellationToken);
    Task UpdateMovieTheaterAsync(MovieTheater movieTheater, CancellationToken cancellationToken);
    Task DeleteMovieTheaterAsync(MovieTheater movieTheater, CancellationToken cancellationToken);
}

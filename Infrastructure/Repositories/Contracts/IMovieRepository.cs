using Domain.Entities;

namespace Infrastructure.Repositories.Contracts;

public interface IMovieRepository
{
    Task InsertMovieAsync(Movie movie,CancellationToken cancellationToken);
    Task InsertMovieAsync(Movie movie, int[] genres, CancellationToken cancellationToken);
    Task<bool> IsDuplicateMovieAsync(string name, CancellationToken cancellationToken);
    Task<Movie?> FindMovieByNameAsync(string name, CancellationToken cancellationToken);
    Task<int> GetMaxOrderOfMovieActorsAsync(CancellationToken cancellationToken);
    Task UpdateMovieAsync(Movie movie, CancellationToken cancellationToken);
    Task<Movie?> FindMovieByIdAsync(int id, CancellationToken cancellationToken);
}

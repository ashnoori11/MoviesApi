using Domain.Entities;

namespace Infrastructure.Repositories.Contracts;

public interface IMovieRepository
{
    Task InsertMovieAsync(Movie movie,CancellationToken cancellationToken);
    Task<bool> IsDuplicateMovieAsync(string name, CancellationToken cancellationToken);
    Task<Movie?> FindMovieByNameAsync(string name, CancellationToken cancellationToken);
    Task<int> GetMaxOrderOfMovieActorsAsync(CancellationToken cancellationToken);
    Task UpdateMovieAsync(Movie movie, CancellationToken cancellationToken);
    Task<Movie?> FindMovieByIdAsync(int id, CancellationToken cancellationToken);
    Task<List<Movie>> GetTopUpCommingMoviesAsync(int top, CancellationToken cancellationToken);
    Task<List<Movie>> GetTopInTheaterMoviesAsync(int top, CancellationToken cancellationToken);
    Task<Movie?> GetFullMovieByIdAsync(int movieId, CancellationToken cancellationToken);
    Task<Movie?> GetJustMovieByIdAsync(int movieId, CancellationToken cancellationToken);
    Task<bool> IsDuplicateMovieAsync(int movieId, string name, CancellationToken cancellationToken);
    Task<Movie?> GetFullMovieByIdNoTrackingAsync(int movieId, CancellationToken cancellationToken);
    Task DeleteMovie(Movie movie, CancellationToken cancellationToken);
}

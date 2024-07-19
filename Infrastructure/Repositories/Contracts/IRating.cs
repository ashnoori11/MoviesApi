using Domain.Entities;

namespace Infrastructure.Repositories.Contracts;

public interface IRatingRepository
{
    Task InsertRatingAsync(Rating rating, CancellationToken cancellationToken);
    Task UpdateRatingAsync(Rating rating, CancellationToken cancellationToken);
    Task DeleteRatingAsync(Rating rating, CancellationToken cancellationToken);

    Task<Rating?> GetRatingByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Rating>> GetRatingsByMovieIdAsync(int movieId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Rating>> GetRatingsByUserIdAsync(string userId, CancellationToken cancellationToken);
    Task<Rating?> GetRatingByUserIdAndMovieIdAsync(string userId, int movieId, CancellationToken cancellationToken);

    Task<bool> HasAnyRateAsync(int movieId, CancellationToken cancellationToken);
    Task<bool> HasAnyRateAsync(int movieId, string userId, CancellationToken cancellationToken);
    Task<bool> HasAnyRateAsync(string userId, CancellationToken cancellationToken);

    Task<double> GetAverageRateByMovieIdAsync(int movieId, CancellationToken cancellationToken);
}

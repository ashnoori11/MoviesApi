using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RatingRepository(MoviesContext context) : IRatingRepository
{
    private readonly MoviesContext _context = context;

    public async Task DeleteRatingAsync(Rating rating, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _context.Ratings.Remove(rating);
        await Task.CompletedTask;
    }

    public async Task<Rating?> GetRatingByIdAsync(int id, CancellationToken cancellationToken)
        => await _context.Ratings.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<Rating?> GetRatingByUserIdAndMovieIdAsync(string userId, int movieId, CancellationToken cancellationToken)
        => await _context.Ratings.FirstOrDefaultAsync(a => a.UserId == userId && a.MovieId == movieId, cancellationToken);

    public async Task<IReadOnlyList<Rating>> GetRatingsByMovieIdAsync(int movieId, CancellationToken cancellationToken)
        => await _context
        .Ratings
        .AsNoTracking()
        .Where(a => a.MovieId == movieId)
        .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Rating>> GetRatingsByUserIdAsync(string userId, CancellationToken cancellationToken)
        => await _context
                .Ratings
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .ToListAsync(cancellationToken);

    public async Task InsertRatingAsync(Rating rating, CancellationToken cancellationToken)
        => await _context.Ratings.AddAsync(rating, cancellationToken);

    public async Task UpdateRatingAsync(Rating rating, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _context.Ratings.Update(rating);
        await Task.CompletedTask;
    }

    public async Task<bool> HasAnyRateAsync(int movieId, CancellationToken cancellationToken)
        => await _context.Ratings.AnyAsync(a => a.MovieId == movieId, cancellationToken);

    public async Task<bool> HasAnyRateAsync(int movieId, string userId, CancellationToken cancellationToken)
        => await _context.Ratings.AnyAsync(a => a.MovieId == movieId && a.UserId == userId, cancellationToken);

    public async Task<bool> HasAnyRateAsync(string userId, CancellationToken cancellationToken)
        => await _context.Ratings.AnyAsync(a => a.UserId == userId, cancellationToken);

    public async Task<double> GetAverageRateByMovieIdAsync(int movieId, CancellationToken cancellationToken)
        => await _context.Ratings.Where(a => a.MovieId == movieId)
        .AverageAsync(x => x.Rate, cancellationToken);
}

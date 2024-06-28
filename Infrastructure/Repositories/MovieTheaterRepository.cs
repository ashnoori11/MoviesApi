using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MovieTheaterRepository : IMovieTheaterRepository
{
    #region constructor
    private readonly MoviesContext _context;
    public MovieTheaterRepository(MoviesContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<MovieTheater?> GetMovieTheaterById(int id, CancellationToken cancellationToken)
        => await _context
        .MovieTheaters
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<MovieTheater?> GetMovieTheaterByIdAsNoTracking(int id, CancellationToken cancellationToken)
         => await _context
        .MovieTheaters
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<List<MovieTheater>> GetMovieTheatersByIdAsNoTracking(int[] ids, CancellationToken cancellationToken)
     => await _context
    .MovieTheaters
    .Where(a => ids.Contains(a.Id))
    .ToListAsync(cancellationToken);


    public async Task InsertMovieTheaterAsync(MovieTheater movieTheater, CancellationToken cancellationToken)
        => await _context.MovieTheaters.AddAsync(movieTheater, cancellationToken);

    public async Task UpdateMovieTheaterAsync(MovieTheater movieTheater, CancellationToken cancellationToken)
    {
        _context.MovieTheaters.Update(movieTheater);
        cancellationToken.ThrowIfCancellationRequested();
        await Task.CompletedTask;
    }

    public async Task DeleteMovieTheaterAsync(MovieTheater movieTheater, CancellationToken cancellationToken)
    {
        _context.MovieTheaters.Remove(movieTheater);
        cancellationToken.ThrowIfCancellationRequested();
        await Task.CompletedTask;
    }
}

using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    #region constructor
    private readonly MoviesContext _context;
    public MovieRepository(MoviesContext context)
    {
        _context = context;
    }
    #endregion

    public async Task InsertMovieAsync(Movie movie, CancellationToken cancellationToken)
        => await _context.Movies.AddAsync(movie, cancellationToken);

    public async Task<bool> IsDuplicateMovieAsync(string name, CancellationToken cancellationToken)
        => await _context.Movies.AnyAsync(a => a.Title == name, cancellationToken);

    public async Task<bool> IsDuplicateMovieAsync(int movieId,string name, CancellationToken cancellationToken)
        => await _context.Movies.AnyAsync(a => a.Id != movieId && a.Title == name, cancellationToken);

    public async Task<List<Movie>> GetTopInTheaterMoviesAsync(int top, CancellationToken cancellationToken)
        => await _context.Movies
        .AsNoTracking()
        .Where(a => a.InTheaters == true)
        .OrderByDescending(a => a.ReleaseDate)
        .Take(top)
        .ToListAsync(cancellationToken);

    public async Task<List<Movie>> GetTopUpCommingMoviesAsync(int top, CancellationToken cancellationToken)
        => await _context.Movies
        .AsNoTracking()
        .Where(a => a.ReleaseDate > DateTime.Now)
        .OrderByDescending(a => a.ReleaseDate)
        .Take(top)
        .ToListAsync(cancellationToken);

    public async Task<Movie?> FindMovieByNameAsync(string name, CancellationToken cancellationToken)
        => await _context
        .Movies
        .Include(a => a.MovieGenres)
        .Include(a => a.MovieTheaterMovies)
        .Include(a => a.MovieActors)
        .FirstOrDefaultAsync(a => a.Title == name, cancellationToken);


    public async Task<Movie?> FindMovieByIdAsync(int id, CancellationToken cancellationToken)
        => await _context
        .Movies
        .Include(a => a.MovieGenres)
        .Include(a => a.MovieTheaterMovies)
        .Include(a => a.MovieActors)
        .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task UpdateMovieAsync(Movie movie, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _context.Update(movie);
        await Task.CompletedTask;
    }

    public async Task<int> GetMaxOrderOfMovieActorsAsync(CancellationToken cancellationToken)
    {
        IQueryable<MovieActors> movieActors = _context
            .MovieActors
            .AsNoTracking();

        if (await movieActors.AnyAsync(cancellationToken))
            return await movieActors.MaxAsync(a => a.Order, cancellationToken);

        return 0;
    }

    public async Task<Movie?> GetJustMovieByIdAsync(int movieId, CancellationToken cancellationToken)
        => await _context.Movies
        .Include(a => a.MovieActors)
        .Include(a => a.MovieTheaterMovies)
        .Include(a => a.MovieGenres)
        .FirstOrDefaultAsync(a => a.Id == movieId, cancellationToken);


    public async Task<Movie?> GetFullMovieByIdAsync(int movieId, CancellationToken cancellationToken)
            => await _context.Movies
            .Include(a => a.MovieActors)
            .Include(a => a.MovieTheaterMovies)
            .Include(a => a.MovieGenres)
            .FirstOrDefaultAsync(a => a.Id == movieId, cancellationToken);

    public async Task<Movie?> GetFullMovieByIdNoTrackingAsync(int movieId, CancellationToken cancellationToken)
        => await _context.Movies
        .AsNoTrackingWithIdentityResolution()
        .Include(a => a.MovieActors)
        .Include(a => a.MovieTheaterMovies)
        .Include(a => a.MovieGenres)
        .FirstOrDefaultAsync(a => a.Id == movieId, cancellationToken);


    public async Task DeleteMovie(Movie movie,CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _context.Movies.Remove(movie);
        await Task.CompletedTask;
    }
}

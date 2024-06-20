﻿using Domain.Entities;
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

    public Task InsertMovieAsync(Movie movie, int[] genres, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

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
}
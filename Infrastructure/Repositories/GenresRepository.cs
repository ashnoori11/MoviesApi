using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GenresRepository : IGenresRepository
{
    private readonly MoviesContext _moviesContext;
    public GenresRepository(MoviesContext moviesContext)
    {
        _moviesContext = moviesContext;
    }

    public async Task DeleteGenreByIdAsync(int genreId, CancellationToken cancellationToken)
    {
        var getGenre = await _moviesContext.Genres.FirstOrDefaultAsync(a => a.Id == genreId, cancellationToken);
        if (getGenre is object)
            _moviesContext.Genres.Remove(getGenre);

        await Task.CompletedTask;
    }
    public async Task DeleteGenreAsync(Genre genre, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _moviesContext.Genres.Remove(genre);
        await Task.CompletedTask;
    }
    public async Task<Genre?> GetGenreByIdAsync(int id, CancellationToken cancellationToken)
        => await _moviesContext.Genres.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    public async Task<int> InsertGenreAsync(Genre genre, CancellationToken cancellationToken)
    {
        var addedResult = await _moviesContext.Genres.AddAsync(genre, cancellationToken);
        return addedResult.Entity.Id;
    }
    public async Task UpdateGenreAsync(Genre genre, CancellationToken cancellationToken)
    {
        var getGenre = await _moviesContext.Genres.FirstOrDefaultAsync(a => a.Id == genre.Id, cancellationToken);
        if (getGenre is object)
        {
            getGenre.UpdateGenreName(genre.Name);
            _moviesContext.Genres.Update(getGenre);
        }

        await Task.CompletedTask;
    }
}

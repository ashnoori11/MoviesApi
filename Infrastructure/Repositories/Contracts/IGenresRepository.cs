using Domain.Entities;

namespace Infrastructure.Repositories.Contracts;

public interface IGenresRepository
{
    Task<Genre?> GetGenreByIdAsync(int id,CancellationToken cancellationToken);
    Task UpdateGenreAsync(Genre genre, CancellationToken cancellationToken);
    Task DeleteGenreByIdAsync(int genreId,CancellationToken cancellationToken);
    Task<int> InsertGenreAsync(Genre genre, CancellationToken cancellationToken);
    Task DeleteGenreAsync(Genre genre, CancellationToken cancellationToken);
    Task<List<Genre>> GetGenresByIdsNotrackingAsync(int[] ids, CancellationToken cancellationToken);
}

using Application.Common.Models;
using Application.ViewModel;
using Infrastructure.UnitOfWorks;
using Mapster;
using MediatR;

namespace Application.Movie.Queries;

public record FilterMoviesQuery(int Page, int RecordsPerPage, string Title, int GenreId, bool InTheaters, bool UpCommingReleases) : IRequest<Result<MovieViewModel>>;
 
public class FilterMoviesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<FilterMoviesQuery, Result<MovieViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<MovieViewModel>> Handle(FilterMoviesQuery request, CancellationToken cancellationToken)
    {
        var movieQueryable = _unitOfWork._context.Movies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            movieQueryable = movieQueryable.Where(a=> a.Title.Contains(request.Title));
        }

        if (request.InTheaters)
        {
            movieQueryable = movieQueryable.Where(a => a.InTheaters == true);
        }

        if (request.UpCommingReleases)
        {
            DateTime today = DateTime.Now;
            movieQueryable = movieQueryable.Where(a => a.ReleaseDate > today);
        }

        if(request.GenreId != 0)
        {
            movieQueryable = movieQueryable.Where(a => a.MovieGenres.Select(x => x.GenreId).Contains(request.GenreId));
        }

        var res = await movieQueryable
            .AsNoTracking()
            .Skip((request.Page - 1) * request.RecordsPerPage)
            .Take(request.RecordsPerPage)
            .OrderBy(a=>a.Title)
            .ToListAsync(cancellationToken);

        return Result<MovieViewModel>.Success(res.Adapt<List<MovieViewModel>>());
    }
}

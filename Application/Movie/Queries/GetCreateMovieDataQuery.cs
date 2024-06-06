using Application.Common.Models;
using Application.Dtos;
using Application.Genre.Queries;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Movie.Queries;

public record GetCreateMovieDataQuery : IRequest<Result<CreateMovieInformationsDto>> ;

public class GetCreateMovieDataQueryHandler : IRequestHandler<GetCreateMovieDataQuery, Result<CreateMovieInformationsDto>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public GetCreateMovieDataQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result<CreateMovieInformationsDto>> Handle(GetCreateMovieDataQuery request, CancellationToken cancellationToken)
    {
        var allGenres = await _unitOfWork
            .GetQueryRepository<Domain.Entities.Genre>()
            .GetSpecificColumsAsync(a=> new GenreDto
            {
                Id = a.Id,
                Name = a.Name
            },cancellationToken);

        var allMovieTheatersDto = await _unitOfWork
            .GetQueryRepository<Domain.Entities.MovieTheater>()
            .GetSpecificColumsAsync(a => new MovieTheatersDto(a.Id,a.Name), cancellationToken);

        var theModel = new CreateMovieInformationsDto(
            allGenres.ToList(),
            allMovieTheatersDto.ToList()
            );

        return Result<CreateMovieInformationsDto>.Success(theModel);
    }
}

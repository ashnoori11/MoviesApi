using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.MovieTheater.Queries;

public record GetMovieTheaterByIdQuery(int MovieTheaterId) : IRequest<Result<MovieTheaterDto>>;

public class GetMovieTheaterByIdQueryHandler : IRequestHandler<GetMovieTheaterByIdQuery, Result<MovieTheaterDto>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public GetMovieTheaterByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result<MovieTheaterDto>> Handle(GetMovieTheaterByIdQuery request, CancellationToken cancellationToken)
    {
        var getMovieTheater = await _unitOfWork
            .MovieTheaterRepository
            .GetMovieTheaterByIdAsNoTracking(request.MovieTheaterId,cancellationToken);

        if (getMovieTheater is null)
            return Result<MovieTheaterDto>.NotFound();

        MovieTheaterDto cast = getMovieTheater;
        return Result<MovieTheaterDto>.Success(cast);
    }
}

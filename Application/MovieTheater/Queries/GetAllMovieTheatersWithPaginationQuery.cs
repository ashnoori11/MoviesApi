using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using Mapster;
using MediatR;

namespace Application.MovieTheater.Queries;

public class GetAllMovieTheatersWithPaginationQuery : IRequest<Result<MovieTheaterDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}


public class GetAllMovieTheatersQueryHandler : IRequestHandler<GetAllMovieTheatersWithPaginationQuery, Result<MovieTheaterDto>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public GetAllMovieTheatersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result<MovieTheaterDto>> Handle(GetAllMovieTheatersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var getResult = await _unitOfWork
            .GetQueryRepository<Domain.Entities.MovieTheater>()
            .GetAllRowsNoTracking(request.PageNumber, request.PageSize, cancellationToken);

            var listAsDto = await MovieTheaterDto.ConvertListAsync(getResult.DataList,cancellationToken);
            return Result<List<MovieTheaterDto>>.Success(listAsDto, getResult.TotalCount);
        }
        catch (Exception exp)
        {
            return Result<MovieTheaterDto>.Failure(new List<string> { exp.Message });
        }
    }
}
using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Genre.Queries;

public record GetAllGenresQuery() : IRequest<Result<GenreDto>>;

public class GetGenresQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllGenresQuery, Result<GenreDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var getAllGenres = await _unitOfWork.GetQueryRepository<Domain.Entities.Genre>()
            .GetSpecificColumsByFilterAsync(a=>a.Id != 0,a=> new GenreDto { Id = a.Id,Name = a.Name},cancellationToken);

        return Result<GenreDto>.Success(getAllGenres.ToList());
    }
}

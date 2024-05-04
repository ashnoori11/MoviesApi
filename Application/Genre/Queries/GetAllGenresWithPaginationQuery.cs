using Application.Common.Models;
using FluentValidation;
using Infrastructure.UnitOfWorks;
using Mapster;
using MediatR;

namespace Application.Genre.Queries;

public record GetAllGenresWithPaginationQuery : IRequest<Result<GenreDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresWithPaginationQuery, Result<GenreDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGenresQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GenreDto>> Handle(GetAllGenresWithPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var getResult = await _unitOfWork
            .GetQueryRepository<Domain.Entities.Genre>()
            .GetAllRowsNoTracking(request.PageNumber, request.PageSize, cancellationToken);

            return Result<GenreDto>.Success(getResult.DataList.Adapt<List<GenreDto>>(), getResult.TotalCount);
        }
        catch (Exception exp)
        {
            return Result<GenreDto>.Failure(new List<string> { exp.Message });
        }
    }
}

public class GetAllGenresWithPaginationQueryValidator : AbstractValidator<GetAllGenresWithPaginationQuery>
{
    public GetAllGenresWithPaginationQueryValidator()
    {
        RuleFor(v => v.PageSize)
            .NotEqual(0)
            .NotEmpty();

        RuleFor(v => v.PageNumber)
            .NotEqual(0)
            .NotEmpty();
    }
}
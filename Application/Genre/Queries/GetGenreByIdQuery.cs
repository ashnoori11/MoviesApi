using Application.Common.Models;
using FluentValidation;
using Infrastructure.UnitOfWorks;
using Mapster;
using MediatR;

namespace Application.Genre.Queries;

public record GetGenreByIdQuery(int GenreId) : IRequest<Result<GenreDto>>;

public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, Result<GenreDto>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public GetGenreByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result<GenreDto>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var getGenre = await _unitOfWork
            .GenreRepository
            .GetGenreByIdAsync(request.GenreId,cancellationToken);

        if (getGenre is null)
            return Result<GenreDto>.Failure($"Not Found Genre By Id : {request.GenreId}");

        return Result<GenreDto>.Success(getGenre.Adapt<GenreDto>());
    }
}


public class GetGenreByIdQueryValidator : AbstractValidator<GetGenreByIdQuery>
{
    public GetGenreByIdQueryValidator()
    {
        RuleFor(v => v.GenreId)
            .NotEqual(0)
            .NotEmpty();
    }
}
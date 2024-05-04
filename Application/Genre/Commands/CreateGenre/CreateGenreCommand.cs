using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Genre.Commands.CreateGenre;

public record CreateGenreCommand : IRequest<Result<int>>
{
    public string? Name { get; set; }
}


public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, Result<int>>
{
    #region constructure
    private readonly IUnitOfWork _unitOfWork;
    public CreateGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result<int>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = Domain.Entities.Genre.SetGenre(request.Name); 
        await _unitOfWork.GenreRepository.InsertGenreAsync(genre,cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<int>.Success();
    }
}
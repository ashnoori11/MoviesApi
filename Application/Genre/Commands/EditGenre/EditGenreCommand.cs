using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Genre.Commands.EditGenre;

public record EditGenreCommand(int Id, string? Name) : IRequest<Result>;

public class EditGenreCommandHandler : IRequestHandler<EditGenreCommand, Result>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public EditGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion


    public async Task<Result> Handle(EditGenreCommand request, CancellationToken cancellationToken)
    {
        var getGenre = await _unitOfWork.GenreRepository.GetGenreByIdAsync(request.Id,cancellationToken);
        if (getGenre is null)
            return Result.Failure($"Can not find Genre By Id : {request.Id}");

        getGenre.UpdateGenreName(request.Name);
        await _unitOfWork.GenreRepository.UpdateGenreAsync(getGenre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

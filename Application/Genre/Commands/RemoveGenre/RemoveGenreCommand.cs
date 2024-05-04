using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Genre.Commands.RemoveGenre;

public record RemoveGenreCommand(int Id) : IRequest<Result>;

public class RemoveGenreCommandHandler : IRequestHandler<RemoveGenreCommand, Result>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public RemoveGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result> Handle(RemoveGenreCommand request, CancellationToken cancellationToken)
    {
        var getGenre = await _unitOfWork.GenreRepository.GetGenreByIdAsync(request.Id, cancellationToken);

        if (getGenre is null)
            return Result.NotFound();

        await _unitOfWork.GenreRepository.DeleteGenreAsync(getGenre,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}


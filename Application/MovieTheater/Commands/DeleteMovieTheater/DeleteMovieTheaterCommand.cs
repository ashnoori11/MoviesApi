using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.MovieTheater.Commands.DeleteMovieTheater;

public record DeleteMovieTheaterCommand(int MovieTheaterId) : IRequest<Result>;
public class DeleteMovieTheaterCommandHandler : IRequestHandler<DeleteMovieTheaterCommand, Result>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public DeleteMovieTheaterCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result> Handle(DeleteMovieTheaterCommand request, CancellationToken cancellationToken)
    {
        var getMovieTheater = await _unitOfWork.MovieTheaterRepository.GetMovieTheaterById(request.MovieTheaterId,cancellationToken);

        if (getMovieTheater is null)
            return Result.NotFound();

        await _unitOfWork.MovieTheaterRepository.DeleteMovieTheaterAsync(getMovieTheater,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
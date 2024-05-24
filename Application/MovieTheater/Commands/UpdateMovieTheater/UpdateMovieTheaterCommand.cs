using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;
using NetTopologySuite.Geometries;

namespace Application.MovieTheater.Commands.UpdateMovieTheater;

public record UpdateMovieTheaterCommand(int MovieTheaterId, string Name, double Latitude, double Longitude) : IRequest<Result>;

public class UpdateMovieTheaterCommandHandler : IRequestHandler<UpdateMovieTheaterCommand, Result>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public UpdateMovieTheaterCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result> Handle(UpdateMovieTheaterCommand request, CancellationToken cancellationToken)
    {
        var getMovieTheater = await _unitOfWork
            .MovieTheaterRepository
            .GetMovieTheaterById(request.MovieTheaterId, cancellationToken);

        if (getMovieTheater is null)
            return Result.NotFound();

        var locationPoint = new Point(request.Longitude, request.Latitude);

        getMovieTheater.SetChanges(request.Name, locationPoint);
        await _unitOfWork.MovieTheaterRepository.UpdateMovieTheaterAsync(getMovieTheater,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

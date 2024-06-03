using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;
using NetTopologySuite;
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

        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var currentLocation = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(request.Latitude, request.Longitude));

        getMovieTheater.SetChanges(request.Name, currentLocation);
        await _unitOfWork.MovieTheaterRepository.UpdateMovieTheaterAsync(getMovieTheater,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

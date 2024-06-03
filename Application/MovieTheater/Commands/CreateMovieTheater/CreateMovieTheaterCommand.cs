using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;
using NetTopologySuite;

namespace Application.MovieTheater.Commands.CreateMovieTheater;

public record CreateMovieTheaterCommand(string Name,double Latitude,double Longitude) : IRequest<Result>;

public class CreateMovieTheaterCommandHandler : IRequestHandler<CreateMovieTheaterCommand, Result>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public CreateMovieTheaterCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result> Handle(CreateMovieTheaterCommand request, CancellationToken cancellationToken)
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var currentLocation = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(request.Latitude, request.Longitude));

        var forInsertObject = new Domain.Entities.MovieTheater(request.Name, currentLocation);
        await _unitOfWork.MovieTheaterRepository.InsertMovieTheaterAsync(forInsertObject,cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

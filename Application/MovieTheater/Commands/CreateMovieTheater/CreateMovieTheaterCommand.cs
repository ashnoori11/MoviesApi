using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;
using NetTopologySuite.Geometries;

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
        var locationPoinr = new Point(request.Longitude,request.Latitude);
        var forInsertObject = new Domain.Entities.MovieTheater(request.Name, locationPoinr);
        await _unitOfWork.MovieTheaterRepository.InsertMovieTheaterAsync(forInsertObject,cancellationToken);

        return Result.Success();
    }
}

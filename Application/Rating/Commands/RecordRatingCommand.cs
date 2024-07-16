using Application.Common.Models;
using Application.Services.IdentityServices.Contracts;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Rating.Commands;

public record RecordRatingCommand(string Email,int MovieId,int Rating) : IRequest<Result>;
public class RecordRatingCommandHandler(IUnitOfWork unitOfWork, IIdentityFactory identityFactory) : IRequestHandler<RecordRatingCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdentityFactory _identityFactory = identityFactory;

    public async Task<Result> Handle(RecordRatingCommand request, CancellationToken cancellationToken)
    {
        var user = await _identityFactory
            .CreateUserManager()
            .FindByEmailAsync(request.Email);

        if (user is null)
            return Result.NotFound();

        var getMovie = await _unitOfWork
            .MovieRepository
            .GetJustMovieByIdAsync(request.MovieId,cancellationToken);

        if (getMovie is null)
            return Result.NotFound();

        var tryFindRateRecord = await _unitOfWork
            .RatingRepository
            .GetRatingByUserIdAndMovieIdAsync(user.Id,getMovie.Id,cancellationToken);

        if(tryFindRateRecord is null)
        {
            var newRate = new Domain.Entities.Rating(request.Rating,getMovie.Id,user.Id);
            await _unitOfWork.RatingRepository.InsertRatingAsync(newRate,cancellationToken);
        }
        else
        {
            tryFindRateRecord.UpdateRate(request.Rating);
            await _unitOfWork.RatingRepository.UpdateRatingAsync(tryFindRateRecord,cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

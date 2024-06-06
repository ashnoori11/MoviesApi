using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using Mapster;
using MediatR;

namespace Application.Actor.Queries;
public record GetActorByIdQuery(int ActorId) : IRequest<Result<ActorsDto>>;

public class GetActorByIdQueryHandler : IRequestHandler<GetActorByIdQuery, Result<ActorsDto>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public GetActorByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result<ActorsDto>> Handle(GetActorByIdQuery request, CancellationToken cancellationToken)
    {
        var getActor = await _unitOfWork.ActorRepository.GetActorByIdAsNoTrackingAsync(request.ActorId, cancellationToken);

        if (getActor is null)
            return Result<ActorsDto>.NotFound();

        var actorModel = new ActorsDto(getActor.Id,getActor.Name,getActor.DateOfBirth,getActor.Biography,getActor.Picture);
        return Result<ActorSearchResultDto>.Success(actorModel);
    }
}

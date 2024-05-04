using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Actor.Commands.UpdateActor;

public record UpdateActorCommand(int ActorId,string Name, DateTime DateOfBirth, string Biography, string Picture) : IRequest<Result>;

public class UpdateActorCommandHandler : IRequestHandler<UpdateActorCommand, Result>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public UpdateActorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
    {
        var getActor = await _unitOfWork.ActorRepository.GetActorByIdAsync(request.ActorId,cancellationToken);
        if (getActor is null)
            return Result.NotFound($"can not find actor with id :{request.ActorId}");

        getActor.SetChanges(request.Name,request.DateOfBirth,request.Biography,request.Picture);
        await _unitOfWork.ActorRepository.UpdateActorAsync(getActor,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

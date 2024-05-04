using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Actor.Commands.CreateActor;

public record CreateActorCommand(string Name, DateTime DateOfBirth, string Biography, string Picture) : IRequest<Result>;

public class CreateActorCommandHandler : IRequestHandler<CreateActorCommand, Result>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public CreateActorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ActorRepository.CreateActorAsync(
            new Domain.Entities.Actor(request.Name, request.DateOfBirth, request.Biography, request.Picture)
            ,cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}



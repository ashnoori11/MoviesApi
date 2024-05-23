using Application.Common.Extensions;
using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Actor.Commands.DeleteActor;

public record DeleteActorCommand(int ActorId,string webRootPath) : IRequest<Result>;

public class DeleteActorCommandHandler : IRequestHandler<DeleteActorCommand, Result>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public DeleteActorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
    {
        var getactor = await _unitOfWork.ActorRepository.GetActorByIdAsync(request.ActorId, cancellationToken);
        if (getactor is null)
            return Result.NotFound($"can not find actor by id : {request.ActorId}");

        await _unitOfWork.ActorRepository.DeleteActorAsync(getactor,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrEmpty(getactor.Picture))
        {
            var imageName = getactor.Picture.ExtractFileNameFromUrl();
            string theImagePath = $"{request.webRootPath}/Images/Actors/{imageName}";

            if (File.Exists(theImagePath))
                File.Delete(theImagePath);
        }

        return Result.Success();
    }
}
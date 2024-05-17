using Application.Common.Models;
using Application.Common.Utilities;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Actor.Commands.UpdateActor;

public record UpdateActorCommand(int ActorId,string Name, DateTime DateOfBirth, string Biography, IFormFile? Picture) : IRequest<Result>;

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

        string imagePath = string.Empty;
        if(request.Picture is object)
        {
            if (!string.IsNullOrWhiteSpace(getActor.Picture))
            {
                File.Delete(getActor.Picture);
            }

            using FileUploader fileUploader = new($"{Directory.GetCurrentDirectory()}/Files/Images/Actors", request.Picture);
            var uploadPictureResult = await fileUploader.UploadImageAsync(cancellationToken);
            if (!uploadPictureResult.Status)
                return Result.Failure($"can not upload image : {uploadPictureResult.Message}");

            imagePath = uploadPictureResult.FileName;
        }

        getActor.SetChanges(request.Name,request.DateOfBirth,request.Biography,imagePath);
        await _unitOfWork.ActorRepository.UpdateActorAsync(getActor,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

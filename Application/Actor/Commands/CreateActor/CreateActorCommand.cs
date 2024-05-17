using Application.Common.Models;
using Application.Common.Utilities;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Actor.Commands.CreateActor;

public record CreateActorCommand(string Name, DateTime DateOfBirth, string Biography, IFormFile Picture,string Url) : IRequest<Result>;

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
        string imagePath = string.Empty;
        if (request.Picture is object)
        {
            using FileUploader fileUploader = new($"{Directory.GetCurrentDirectory()}/Files/Images/Actors", request.Picture);
            var uploadPictureResult = await fileUploader.UploadImageAsync(request.Url, "/Files/Images/Actors", cancellationToken);

            if (!uploadPictureResult.Status)
                return Result.Failure($"can not upload image : {uploadPictureResult.Message}");

            imagePath = uploadPictureResult.FileRoute;
        }

        await _unitOfWork.ActorRepository.CreateActorAsync(
            new Domain.Entities.Actor(request.Name, request.DateOfBirth, request.Biography, imagePath)
            ,cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}



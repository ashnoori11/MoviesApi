using Application.Common.Models;
using Infrastructure.UnitOfWorks;
using MediatR;
using Application.Common.Extensions;

namespace Application.Movie.Commands.DeleteMovie;

public record DeleteMovieCommand(int Id,string webRootPath) : IRequest<Result>;

public class DeleteMovieCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMovieCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var getTheMovie = await _unitOfWork.MovieRepository.GetFullMovieByIdAsync(request.Id, cancellationToken);
        if (getTheMovie is null)
            return Result.NotFound();

        string thePoster = string.Empty;

        if (!string.IsNullOrWhiteSpace(getTheMovie.Poster))
            thePoster = getTheMovie.Poster.ExtractFileNameByUrl();

        await _unitOfWork.MovieRepository.DeleteMovie(getTheMovie, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(thePoster))
            File.Delete($"{request.webRootPath}/Images/Actors/{thePoster}");

        return Result.Success();
    }
}

//DECLARE @url VARCHAR(MAX) = the url
//DECLARE @fileName VARCHAR(MAX)
//SET @fileName = REVERSE(SUBSTRING(REVERSE(@url), 1, CHARINDEX('/', REVERSE(@url)) - 1))
//print(@fileName)

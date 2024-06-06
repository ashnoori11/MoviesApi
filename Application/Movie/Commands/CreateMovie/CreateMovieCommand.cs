using Application.Common.Models;
using Application.Common.Utilities;
using Application.Dtos;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Movie.Commands.CreateMovie;

public record CreateMovieCommand(
    string Title,
    string Summery,
    string Trailer,
    bool InTheaters,
    DateTime? ReleaseDate,
    IFormFile Poster,
    List<int> GenresIds,
    List<int> MovieTheatersIds,
    List<MoviesActorsFormDto> Actors,
    string WebRootPath,
    string Url) 
    : IRequest<Result<int>>;


public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Result<int>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public CreateMovieCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    #endregion

    public async Task<Result<int>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var isExistBefor = await _unitOfWork.MovieRepository.IsDuplicateMovieAsync(request.Title,cancellationToken);
        if (isExistBefor)
            return Result<int>.Failure("duplicate movie");

        string imagePath = string.Empty;
        if (request.Poster is object)
        {
            using FileUploader fileUploader = new($"{request.WebRootPath}/Images/Actors", request.Poster);
            var uploadPictureResult = await fileUploader.UploadImageAsync(request.Url, "/Images/Actors", cancellationToken);

            if (!uploadPictureResult.Status)
                return Result<int>.Failure($"can not upload image : {uploadPictureResult.Message}");

            imagePath = uploadPictureResult.FileRoute;
        }

        var movie = new Domain.Entities.Movie(request.Title,request.Summery,request.Trailer,request.InTheaters,request.ReleaseDate, imagePath);
        await _unitOfWork.MovieRepository.InsertMovieAsync(movie,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var getMovie = await _unitOfWork.MovieRepository.FindMovieByNameAsync(request.Title,cancellationToken);
        if (getMovie is null)
            return Result<int>.NotFound();

        foreach (var item in request.GenresIds)
        {
            getMovie.SetMovieGenres(new MovieGenres(getMovie.Id,item));
        }

        foreach (var item in request.MovieTheatersIds)
        {
            getMovie.SetTheaterMovies(new MovieTheaterMovies(item,getMovie.Id));
        }

        var getMovieActorsLastOrder = await _unitOfWork.MovieRepository.GetMaxOrderOfMovieActorsAsync(cancellationToken);
        foreach (var item in request.Actors)
        {
            getMovie.SetMovieActors(new MovieActors(getMovie.Id,item.Id,item.Character, (getMovieActorsLastOrder + 1)));
        }

        await _unitOfWork.MovieRepository.UpdateMovieAsync(getMovie,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(getMovie.Id);
    }
}
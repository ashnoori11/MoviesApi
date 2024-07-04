using Application.Common.Extensions;
using Application.Common.Models;
using Application.Common.Utilities;
using Application.Dtos;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Movie.Commands.EditMovie;

public record EditMovieCommand(
    int Id,
    string Title,
    string Summery,
    string Trailer,
    bool InTheaters,
    DateTime? ReleaseDate,
    IFormFile? Poster,
    List<int> GenresIds,
    List<int> MovieTheatersIds,
    List<MoviesActorsFormDto> Actors,
    string WebRootPath,
    string Url)
    : IRequest<Result<int>>;


public class EditMovieCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<EditMovieCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<int>> Handle(EditMovieCommand request, CancellationToken cancellationToken)
    {
        var getTheMovie = await _unitOfWork.MovieRepository.GetFullMovieByIdAsync(request.Id, cancellationToken);
        if (getTheMovie is null)
            return Result<int>.NotFound();

        bool hasDuplicateName = await _unitOfWork.MovieRepository.IsDuplicateMovieAsync(getTheMovie.Id, request.Title, cancellationToken);
        if (hasDuplicateName)
            return Result<int>.Failure("the title is duplicate");

        getTheMovie.SetChanges(request.Title, request.Summery, request.Trailer);

        string recentPoster = string.Empty;
        if (request.Poster is object)
        {
            using FileUploader fileUploader = new($"{request.WebRootPath}/Images/Actors", request.Poster);
            var uploadPictureResult = await fileUploader.UploadImageAsync(request.Url, "/Images/Actors", cancellationToken);

            if (!uploadPictureResult.Status)
                return Result<int>.Failure($"can not upload image : {uploadPictureResult.Message}");

            recentPoster = getTheMovie.Poster.ExtractFileNameByUrl();
            getTheMovie.SetPoster(uploadPictureResult.FileRoute);
        }

        getTheMovie.SetReleaseDate(request.ReleaseDate);

        // genres
        getTheMovie.MovieGenres.Clear();
        List<MovieGenres> newMovieGenres = request.GenresIds
            .Select(a => new MovieGenres(getTheMovie.Id, a))
            .ToList();

        newMovieGenres.ForEach(item => getTheMovie.SetMovieGenres(item));

        // actors
        getTheMovie.MovieActors.Clear();
        List<MovieActors> newMovieActors = request.Actors
            .Select((a, index) => new MovieActors(getTheMovie.Id, a.Id, a.Character, index + 1))
            .ToList();

        newMovieActors.ForEach(item => getTheMovie.SetMovieActors(item));

        // movie theaters
        getTheMovie.MovieTheaterMovies.Clear();
        List<MovieTheaterMovies> newMovieTheaterMovies = request.MovieTheatersIds
            .Select(a => new MovieTheaterMovies(a, getTheMovie.Id))
            .ToList();

        newMovieTheaterMovies.ForEach(item => getTheMovie.SetTheaterMovies(item));

        await _unitOfWork.MovieRepository.UpdateMovieAsync(getTheMovie, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(recentPoster))
            File.Delete($"{request.WebRootPath}/Images/Actors/{recentPoster}");

        return Result<int>.Success(getTheMovie.Id);
    }
}
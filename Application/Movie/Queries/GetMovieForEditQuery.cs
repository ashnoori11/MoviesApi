using Application.Common.Models;
using Application.Dtos;
using Application.Genre.Queries;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Movie.Queries;

public record GetMovieForEditQuery(int Id) : IRequest<Result<MovieDetailForEditDto>>;
public class GetMovieForEditQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMovieForEditQuery, Result<MovieDetailForEditDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<MovieDetailForEditDto>> Handle(GetMovieForEditQuery request, CancellationToken cancellationToken)
    {
        var getTheMovie = await _unitOfWork
            .MovieRepository
            .GetFullMovieByIdNoTrackingAsync(request.Id, cancellationToken);

        if (getTheMovie is null)
            return Result<MovieDetailForEditDto>.NotFound();

        int[] actorIdies = getTheMovie.MovieActors.Select(a => a.ActorId).ToArray();
        var getActors = await _unitOfWork.ActorRepository.GetActorsByIdsAsNoTrackingAsync(actorIdies, cancellationToken);

        int[] selectedGenresIdies = getTheMovie.MovieGenres.Select(a => a.GenreId).ToArray();

        var selectedGenrs = await _unitOfWork
            .GetQueryRepository<Domain.Entities.Genre>()
            .GetSpecificColumsByFilterAndOrderByAsync(a => selectedGenresIdies.Contains(a.Id), a => new GenreDto { Id = a.Id, Name = a.Name }, cancellationToken);

        var nonSelectedGenres = await _unitOfWork
            .GetQueryRepository<Domain.Entities.Genre>()
            .GetSpecificColumsByFilterAndOrderByAsync(a => !selectedGenresIdies.Contains(a.Id), a => new GenreDto { Id = a.Id, Name = a.Name }, cancellationToken);

        var selectedActors = getActors
            .Select(a => new MoviesActorsFormDto { Id = a.Id, Name = a.Name, Picture = a.Picture, Character = a.MovieActors.FirstOrDefault(a => a.ActorId == a.ActorId)?.Character })
            .ToList();

        int[] selectedMovieTheaterIdies = getTheMovie.MovieTheaterMovies.Select(a => a.MovieTheaterId).ToArray();
        var selectedMovieTheaters = await _unitOfWork
            .GetQueryRepository<Domain.Entities.MovieTheater>()
            .GetSpecificColumsByFilterAndOrderByAsync(a => selectedGenresIdies.Contains(a.Id), a => 
            new MovieTheaterDetailDto { Id = a.Id, Name = a.Name,Latitude = a.Location.Y,Longitude = a.Location.X}, cancellationToken);

        var nonSelectedMovieTheaters = await _unitOfWork
            .GetQueryRepository<Domain.Entities.MovieTheater>()
            .GetSpecificColumsByFilterAndOrderByAsync(a => !selectedGenresIdies.Contains(a.Id), a =>
            new MovieTheaterDetailDto { Id = a.Id, Name = a.Name, Latitude = a.Location.Y, Longitude = a.Location.X }, cancellationToken);

        var resultDetails = new MovieDetailForEditDto
        {
            Id = getTheMovie.Id,
            Title = getTheMovie.Title,
            InTheaters = getTheMovie.InTheaters,
            Poster = getTheMovie.Poster,
            PosterURL = getTheMovie.Poster,
            ReleaseDate = getTheMovie.ReleaseDate,
            Trailer = getTheMovie.Trailer,
            SelectedActors = selectedActors,
            Summery = getTheMovie.Summery,
            SelectedGenre = selectedGenrs.ToList(),
            NonSelectedGenre = nonSelectedGenres.ToList(),
            SelectedMovieTheaters = selectedMovieTheaters.ToList(),
            NonSelectedMovieTheaters = nonSelectedMovieTheaters.ToList()
        };

        return Result<MovieDetailForEditDto>.Success(resultDetails);
    }
}

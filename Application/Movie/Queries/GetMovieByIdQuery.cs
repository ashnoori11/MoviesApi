using Application.Common.Models;
using Application.Dtos;
using Application.Genre.Queries;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Movie.Queries;

public record GetMovieByIdQuery(int Id) : IRequest<Result<MovieDetailDto>>;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Result<MovieDetailDto>>
{
    #region constructor
    private readonly IUnitOfWork _unitOfWork;
    public GetMovieByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MovieDetailDto>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var getMovie = await _unitOfWork.MovieRepository.FindMovieByIdAsync(request.Id, cancellationToken);
        if (getMovie is null)
            return Result<MovieDetailDto>.NotFound();

        var dto = new MovieDetailDto
        {
            Id = getMovie.Id,
            Title = getMovie.Title,
            Summery = getMovie.Summery,
            Trailer = getMovie.Trailer,
            InTheaters = getMovie.InTheaters,
            ReleaseDate = getMovie.ReleaseDate,
            Poster = getMovie.Poster,
        };

        int[] relatedGenresId = getMovie.MovieGenres.Select(a => a.GenreId).ToArray();
        var getRelatedGenres = await _unitOfWork.GenreRepository.GetGenresByIdsNotrackingAsync(relatedGenresId, cancellationToken);

        int[] relatedMovieTheaterIds = getMovie.MovieTheaterMovies.Select(a => a.MovieTheaterId).ToArray();
        var getRelatedMovieTheaters = await _unitOfWork.MovieTheaterRepository.GetMovieTheatersByIdAsNoTracking(relatedMovieTheaterIds, cancellationToken);

        int[] relatedMoviesActorsids = getMovie.MovieActors.Select(a => a.ActorId).ToArray();
        var getRelatedMoviesActors = await _unitOfWork.ActorRepository.GetActorsByIsdNoTrackingAsync(relatedMoviesActorsids, cancellationToken);

        dto.Genres = getRelatedGenres.Select(a => new GenreDto { Id = a.Id, Name = a.Name }).ToList();
        dto.MovieTheaters = getRelatedMovieTheaters.Select(a => new MovieTheaterDetailDto
        {
            Id = a.Id,
            Name = a.Name,
            Latitude = a.Location.Y,
            Longitude = a.Location.X,
        }).ToList();

        List<MoviesActorsFormDto> moviesActorsDtoList = new();
        foreach (var item in getRelatedMoviesActors)
        {
            var getCharacter = getMovie.MovieActors.FirstOrDefault(a => a.ActorId == item.Id);
            string character = string.Empty;

            if (getCharacter is object)
                character = getCharacter.Character ?? string.Empty;

            moviesActorsDtoList.Add(new MoviesActorsFormDto { Id = item.Id,Name = item.Name,Picture = item.Picture,Character = character });
        }

        dto.Actors = moviesActorsDtoList;
        return Result<MovieDetailDto>.Success(dto);
    }
    #endregion
}

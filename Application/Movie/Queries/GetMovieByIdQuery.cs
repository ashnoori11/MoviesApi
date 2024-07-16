using Application.Common.Models;
using Application.Dtos;
using Application.Genre.Queries;
using Application.Services.IdentityServices.Contracts;
using Infrastructure.UnitOfWorks;
using MediatR;

namespace Application.Movie.Queries;

public record GetMovieByIdQuery(int Id, string? UserEmail) : IRequest<Result<MovieDetailDto>>;

public class GetMovieByIdQueryHandler(IUnitOfWork unitOfWork, IIdentityFactory identityFactory) : IRequestHandler<GetMovieByIdQuery, Result<MovieDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdentityFactory _identityFactory = identityFactory;

    public async Task<Result<MovieDetailDto>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var getMovie = await _unitOfWork.MovieRepository.FindMovieByIdAsync(request.Id, cancellationToken);
        if (getMovie is null)
            return Result<MovieDetailDto>.NotFound();

        double averageVote = 0.0;
        int userVote = 0;

        var hasAnyRate = await _unitOfWork.RatingRepository.HasAnyRateAsync(getMovie.Id, cancellationToken);
        if (hasAnyRate)
        {
            averageVote = await _unitOfWork
                .RatingRepository
                .GetAverageRateByMovieIdAsync(getMovie.Id, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.UserEmail))
            {
                var getUser = await _identityFactory.CreateUserManager().FindByEmailAsync(request.UserEmail);
                if (getUser is object)
                {
                    var getUserVote = await _unitOfWork
                        .RatingRepository
                        .GetRatingByUserIdAndMovieIdAsync(getUser.Id,getMovie.Id,cancellationToken);

                    userVote = getUserVote.Rate;
                }
            }
        }

        var dto = new MovieDetailDto
        {
            Id = getMovie.Id,
            Title = getMovie.Title,
            Summery = getMovie.Summery,
            Trailer = getMovie.Trailer,
            InTheaters = getMovie.InTheaters,
            ReleaseDate = getMovie.ReleaseDate,
            Poster = getMovie.Poster,
            AverageVote = averageVote,
            UserVote = userVote,
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

            moviesActorsDtoList.Add(new MoviesActorsFormDto { Id = item.Id, Name = item.Name, Picture = item.Picture, Character = character });
        }

        dto.Actors = moviesActorsDtoList;
        return Result<MovieDetailDto>.Success(dto);
    }
}

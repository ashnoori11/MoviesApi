using Application.Genre.Queries;

namespace Application.Dtos;

public record CreateMovieInformationsDto(List<GenreDto> Genres, List<MovieTheatersDto> MovieTheaters);

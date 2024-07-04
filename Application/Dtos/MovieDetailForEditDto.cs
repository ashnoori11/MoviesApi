using Application.Dtos;
using Application.Genre.Queries;
using System.Text.Json.Serialization;

namespace Application.Dtos;

public record MovieDetailForEditDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool InTheaters { get; set; }
    public string Trailer { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string Poster { get; set; }
    public string PosterURL { get; set; }
    public string Summery { get; set; }

    [JsonPropertyName("nonSelectedGenre")]
    public List<GenreDto> NonSelectedGenre { get; set; }

    [JsonPropertyName("selectedGenre")]
    public List<GenreDto> SelectedGenre { get; set; }

    [JsonPropertyName("nonSelectedMovieTheaters")]
    public List<MovieTheaterDetailDto> NonSelectedMovieTheaters { get; set; }

    [JsonPropertyName("selectedMovieTheaters")]
    public List<MovieTheaterDetailDto> SelectedMovieTheaters { get; set; }

    [JsonPropertyName("selectedActors")]
    public List<MoviesActorsFormDto> SelectedActors { get; set; }
}

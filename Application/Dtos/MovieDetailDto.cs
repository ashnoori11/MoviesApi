using Application.Genre.Queries;

namespace Application.Dtos;

public class MovieDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summery { get; set; }
    public string Trailer { get; set; }
    public bool InTheaters { get; set; } = false;
    public DateTime? ReleaseDate { get; set; } = null;
    public string Poster { get; set; }
    public List<GenreDto> Genres { get; set; }
    public List<MovieTheaterDetailDto> MovieTheaters { get; set; }
    public List<MoviesActorsFormDto> Actors { get; set; }

    public double AverageVote { get; set; }
    public int UserVote { get; set; }
}

namespace Domain.Entities;

public class Movie
{
    private Movie()
    {
            
    }

    public Movie(string title, string summery, string trailer, bool inTheaters, DateTime? releaseDate, string poster)
    {
        Title = title;
        Summery = summery;
        Trailer = trailer;
        InTheaters = inTheaters;
        ReleaseDate = releaseDate;
        Poster = poster;
    }

    public Movie(string title, string summery, string trailer, bool inTheaters, DateTime releaseDate)
    {
        Title = title;
        Summery = summery;
        Trailer = trailer;
        InTheaters = inTheaters;
        ReleaseDate = releaseDate;
    }

    public void SetPoster(string poster) => Poster = poster;
    public void SetReleaseDate(DateTime? releaseDate) => ReleaseDate = releaseDate;
    public void SetMovieStatus(bool isTheater) => InTheaters = isTheater;
    public void SetChanges(string title, string summery, string trailer, bool inTheaters, DateTime? releaseDate, string poster)
    {
        Title = title;
        Summery = summery;
        Trailer = trailer;
        InTheaters = inTheaters;
        ReleaseDate = releaseDate;
        Poster = poster;
    }
    public void SetChanges(string title, string summery, string trailer)
    {
        Title = title;
        Summery = summery;
        Trailer = trailer;
    }

    public void SetMovieGenres(MovieGenres movieGenres)
    {
        if (MovieGenres is null)
            MovieGenres = new List<MovieGenres>();

        MovieGenres.Add(movieGenres);
    }

    public void SetTheaterMovies(MovieTheaterMovies movieTheaterMovies)
    {
        if (MovieTheaterMovies is null)
            MovieTheaterMovies = new List<MovieTheaterMovies>();

        MovieTheaterMovies.Add(movieTheaterMovies);
    }

    public void SetMovieActors(MovieActors movieActors)
    {
        if (MovieActors is null)
            MovieActors = new List<MovieActors>();

        MovieActors.Add(movieActors);
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Summery { get; private set; }
    public string Trailer { get; private set; }
    public bool InTheaters { get; private set; }
    public DateTime? ReleaseDate { get; private set; }
    public string Poster { get; private set; }

    public virtual ICollection<MovieGenres> MovieGenres { get; private set; }
    public virtual ICollection<MovieTheaterMovies> MovieTheaterMovies { get; private set; }
    public virtual ICollection<MovieActors> MovieActors { get; private set; }
    public virtual ICollection<Rating> Ratings { get; private set; }
}

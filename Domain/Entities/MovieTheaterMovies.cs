namespace Domain.Entities;

public class MovieTheaterMovies
{
    private MovieTheaterMovies()
    {
            
    }

    public MovieTheaterMovies(int movieTheaterId, int movieId)
    {
        MovieTheaterId = movieTheaterId;
        MovieId = movieId;
    }

    public int MovieTheaterId { get; private set; }
    public int MovieId { get; private set; }

    public virtual MovieTheater MovieTheater { get; private set; }
    public virtual Movie Movie { get; private set; }
}

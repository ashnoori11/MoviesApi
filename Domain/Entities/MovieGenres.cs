namespace Domain.Entities;

public class MovieGenres
{
    private MovieGenres()
    {
            
    }

    public MovieGenres(int movieId, int genreId)
    {
        MovieId = movieId;
        GenreId = genreId;  
    }

    public int MovieId { get; private set; }
    public int GenreId { get; private set; }

    public virtual Movie Movie { get; private set; }
    public virtual Genre Genre { get; private set; }
}

namespace Domain.Entities;

public class MovieActors
{
    private MovieActors()
    {

    }

    public MovieActors(int movieId, int actorId, string character, int order)
    {
        MovieId = movieId;
        ActorId = actorId;
        Character = character;
        Order = order;

    }

    public int MovieId { get; private set; }
    public int ActorId { get; private set; }

    public string Character { get; private set; }
    public int Order { get; set; }

    public virtual Movie Movie { get; private set; }
    public virtual Actor Actor { get; private set; }
}

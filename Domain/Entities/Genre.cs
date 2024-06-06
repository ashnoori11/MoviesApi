using Domain.Exceptions;

namespace Domain.Entities;

public class Genre
{
    private Genre()
    {
            
    }

    private Genre(string name)
    {
        Name = name;
    }

    public static Genre SetGenre(string name)
    {
        RequiredStringException.ThrowIfNullOrEmpty(name);
        return new Genre(name);
    }

    public void UpdateGenreName(string newName) => Name = newName;
    public int Id { get; private set; }
    public string Name { get; private set; }
    public byte[] RowVersion { get; private set; }

    public virtual ICollection<MovieGenres> MovieGenres { get; private set; }
}

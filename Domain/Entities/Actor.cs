namespace Domain.Entities;

public class Actor
{
    private Actor()
    {
            
    }

    public Actor(string Name, DateTime DateOfBirth, string Biography, string Picture)
    {
        this.Name = Name;
        this.DateOfBirth = DateOfBirth;
        this.Biography = Biography;
        this.Picture= Picture;
    }

    public void SetChanges(string Name, DateTime DateOfBirth, string Biography, string Picture)
    {
        this.Name = Name;
        this.DateOfBirth = DateOfBirth;
        this.Biography = Biography;
        this.Picture = Picture;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string Biography { get; private set; }
    public string Picture { get; private set; }
    public byte[] RowVersion { get; private set; }
}

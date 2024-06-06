namespace Application.Actor.Queries;

public record ActorsDto
    (
    int Id,
    string Name,
    DateTime DateOfBirth,
    string Biography,
    string PictureUrl
    );


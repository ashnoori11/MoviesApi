using System.Collections.Concurrent;

namespace Application.MovieTheater.Queries;

public class MovieTheaterDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Latitude { get; set; }
    public string Longitude { get; set; }


    public static implicit operator MovieTheaterDto(Domain.Entities.MovieTheater? model)
    {
        if (model is null)
            return new MovieTheaterDto();

        return new MovieTheaterDto
        {
            Id = model.Id,
            Name = model.Name,
            Latitude = model.Location.Y.ToString(),
            Longitude = model.Location.X.ToString()
        };
    }

    public async static Task<List<MovieTheaterDto>> ConvertListAsync(IEnumerable<Domain.Entities.MovieTheater> domainModels,CancellationToken cancellationToken)
    {
        ConcurrentBag<MovieTheaterDto> dtos = new();

        await Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            Parallel.ForEach(domainModels, async domainModel =>
            {
                dtos.Add(domainModel);
            });
        });

        return dtos.ToList();
    }
}
namespace Application.ViewModel;

public class MovieViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summery { get; set; }
    public string Trailer { get; set; }
    public bool InTheaters { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string Poster { get; set; }

    public static explicit operator MovieViewModel(Domain.Entities.Movie model)
    {
        return new MovieViewModel
        {
            Id = model.Id,
            InTheaters = model.InTheaters,
            Poster = model.Poster,
            ReleaseDate = model.ReleaseDate,
            Summery = model.Summery,
            Title = model.Title,
            Trailer = model.Trailer
        };
    }

    public static List<MovieViewModel> ConvertDomainModelToViewModelList(List<Domain.Entities.Movie> records)
        => records.Select(a => (MovieViewModel)a).ToList();
}

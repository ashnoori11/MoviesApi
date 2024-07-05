using Application.Movie.Queries;

namespace Application.Dtos;

public class FilterMoviesDto
{
    public int Page { get; set; }
    public int RecordsPerPage { get; set; }
    public PaginationDto PaginationDto {
        get
        {
            return new PaginationDto() { PageNumber = Page,PageSize = RecordsPerPage};
        }
    }

    public string Title { get; set; }
    public int GenreId { get; set; }
    public bool InTheaters { get; set; }
    public bool UpCommingReleases { get; set; }

    public static explicit operator FilterMoviesQuery(FilterMoviesDto model)
        => new FilterMoviesQuery(model.Page, model.RecordsPerPage, model.Title, model.GenreId, model.InTheaters, model.UpCommingReleases);
}

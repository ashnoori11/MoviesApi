using Application.Genre.Queries;

namespace Application.Dtos;

public class PaginationDto
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public static implicit operator GetAllGenresWithPaginationQuery(PaginationDto? model)
    {
        if (model is null)
            return new GetAllGenresWithPaginationQuery();

        return new GetAllGenresWithPaginationQuery
        {
            PageNumber = model.PageNumber,
            PageSize = model.PageSize
        };
    }
}

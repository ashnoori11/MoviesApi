using Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos;

public record MovieFormDto
{
    public int? Id { get; set; }

    [Required, MaxLength(74)]
    public string Title { get; set; }

    [Required, MaxLength(999)]
    public string Summery { get; set; }

    [Required, MaxLength(749)]
    public string Trailer { get; set; }

    public bool InTheaters { get; set; } = false;
    public DateTime? ReleaseDate { get; set; } = null;

    [Required]
    public IFormFile Poster { get; set; }

    [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
    public List<int> GenreIds { get; set; }

    [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
    public List<int> MovieTheaterIds { get; set; }

    [ModelBinder(BinderType = typeof(TypeBinder<List<MoviesActorsFormDto>>))]
    public List<MoviesActorsFormDto> Actors { get; set; }
}

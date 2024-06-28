using Application.Dtos;
using Application.Movie.Commands.CreateMovie;
using Application.Movie.Commands.EditMovie;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoviesApi.Dtos;

public record EditMovieFormDto
{
    [Required]
    public int Id { get; set; }

    [Required, MaxLength(74)]
    public string Title { get; set; }

    [Required, MaxLength(999)]
    public string Summery { get; set; }

    [Required, MaxLength(749)]
    public string Trailer { get; set; }

    public bool InTheaters { get; set; } = false;

    [JsonPropertyName("releaseDate")]
    public DateTime? ReleaseDate { get; set; } = null;

    [Required]
    public IFormFile? Poster { get; set; }

    [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
    public List<int> GenreIds { get; set; }

    [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
    public List<int> MovieTheaterIds { get; set; }

    [ModelBinder(BinderType = typeof(TypeBinder<List<MoviesActorsFormDto>>))]
    public List<MoviesActorsFormDto> Actors { get; set; }

    public EditMovieCommand ConvertToCreateMovieCommand(EditMovieFormDto model, string rootPath, string url)
    {
        return new EditMovieCommand(
                model.Id,
                model.Title,
                model.Summery,
                model.Trailer,
                model.InTheaters,
                model.ReleaseDate,
                model.Poster,
                model.GenreIds,
                model.MovieTheaterIds,
                model.Actors,
                rootPath,
                url
                );
    }
}
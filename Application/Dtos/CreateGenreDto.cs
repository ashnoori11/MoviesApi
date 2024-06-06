using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record CreateGenreDto
{
    [Required(ErrorMessage ="{0} is required")]
    [MinLength(3,ErrorMessage ="{0} can not be less than {1} characters")]
    [MaxLength(150,ErrorMessage ="{0} is invalid - allowed charcters length is {1}")]
    public string? Name { get; set; }
}

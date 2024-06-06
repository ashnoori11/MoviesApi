using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record MovieTheatersCreateDto
{
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(75,ErrorMessage = "{0} can not be more than {1} characters")]
    public string Name { get; set; }

    [Range(-90,90)]
    public double Latitude { get; set; }

    [Range(-180,180)]
    public double Longitude { get; set; }
}

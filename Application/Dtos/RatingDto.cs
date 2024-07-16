using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class RatingDto
{
    [Range(1,5),Required]
    public int Rating { get; set; }

    [Required]
    public int MovieId { get; set; }
}

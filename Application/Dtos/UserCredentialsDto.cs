using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public class UserCredentialsDto
{
    [Required,EmailAddress]
    public string Email { get; set; }

    [Required,
        MinLength(6,ErrorMessage ="{0} can not be less than {1} characters"),
        MaxLength(25,ErrorMessage ="{0} can not be more that {1}")]
    public string Password { get; set; }
}

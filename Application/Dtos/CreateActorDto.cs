﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record CreateActorDto
{
    [Required(ErrorMessage ="{0} is reqiured")]
    [MinLength(2,ErrorMessage ="invalid {0} length")]
    [MaxLength(100,ErrorMessage ="invalid {0} lenfth")]
    public string Name { get; set; }

    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "{0} is reqiured")]
    [MaxLength(849, ErrorMessage = "invalid {0} lenfth")]
    public string Biography { get; set; }

    public IFormFile? Picture { get; set; } = null;
}

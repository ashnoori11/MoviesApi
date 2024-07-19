using Application.Dtos;
using Application.Rating.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RatingsController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RatingDto model, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediator
                .Send(new RecordRatingCommand(CurrentUserEmail, model.MovieId, model.Rating), cancellationToken);

            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }
}

using Application.Dtos;
using Application.MovieTheater.Commands.CreateMovieTheater;
using Application.MovieTheater.Commands.DeleteMovieTheater;
using Application.MovieTheater.Commands.UpdateMovieTheater;
using Application.MovieTheater.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MoviesApi.Filters;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieTheatersController : BaseController
{
    #region constructor
    private readonly IMediator _mediatR;
    public MovieTheatersController(IMediator mediatR)
    {
        _mediatR = mediatR;
    }
    #endregion

    [HttpGet(Name = "GetMovieTheaters")]
    [OutputCache(PolicyName = "Paginating")]
    public async Task<IActionResult> Get([FromQuery] PaginationDto pagingDto, CancellationToken cancellationToken)
    {
        try
        {
            var getGenres = await _mediatR.Send(pagingDto.Adapt<GetAllMovieTheatersWithPaginationQuery>(), cancellationToken);
            return Ok(getGenres);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }

    [HttpGet("{movieTheaterId:int}")]
    [OutputCache(PolicyName = "SingleRow")]
    public async Task<IActionResult> Get(int movieTheaterId, CancellationToken cancellationToken)
    {
        try
        {
            var getGenres = await _mediatR.Send(new GetMovieTheaterByIdQuery(movieTheaterId), cancellationToken);
            return Ok(getGenres);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }

    [HttpPut("{movieTheaterId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
    [ModelStateValidationFilter]
    public async Task<IActionResult> Put(int movieTheaterId, [FromBody] MovieTheatersCreateDto movieTheater, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new UpdateMovieTheaterCommand(movieTheaterId, movieTheater.Name, movieTheater.Latitude, movieTheater.Longitude)
                , cancellationToken);

            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }

    [HttpPost(Name = "CreateMovieTheater")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
    [ModelStateValidationFilter]
    public async Task<IActionResult> Post([FromBody] MovieTheatersCreateDto movieTheater, CancellationToken cancellationToken)
    {
        try
        {
            var getResult = await _mediatR.Send(movieTheater.Adapt<CreateMovieTheaterCommand>(), cancellationToken);
            return Ok(getResult);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediatR.Send(new DeleteMovieTheaterCommand(id), cancellationToken);

            if (result.IsNotFound)
                return NotFound($"can not find Movie Theater with id : {id}");

            return Ok(result);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }

}

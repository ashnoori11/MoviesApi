using Application.Dtos;
using Application.Movie.Commands.DeleteMovie;
using Application.Movie.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MoviesApi.Dtos;
using MoviesApi.FileStorageServices.Liara;
using MoviesApi.Filters;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController(IMediator mediatR,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) : BaseController
{
    private readonly IMediator _mediatR = mediatR;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    [HttpGet, Route("MovieFormInformations")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> MovieFormInformations(CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new GetCreateMovieDataQuery(), cancellationToken);
            return Ok(res.Data);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpGet, Route("{id:int}")]
    [OutputCache(PolicyName = "SingleRow")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        try
        {
            var movieData = await _mediatR.Send(new GetMovieByIdQuery(id,CurrentUserEmail), cancellationToken);
            return Ok(movieData.Data);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpGet, Route("GetMovieDetailsForEdit/{id:int}")]
    [OutputCache(PolicyName = "SingleRow")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetMovieDetailsForEdit(int id, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new GetMovieForEditQuery(id), cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new LandingPageMoviesQuery(6), cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpGet, Route("Filter")]
    [AllowAnonymous]
    public async Task<IActionResult> Filter([FromQuery] FilterMoviesDto model, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send((FilterMoviesQuery)model, cancellationToken);
            return Ok(res.Data);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpPost(Name = "CreateNewMovie")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ModelStateValidationFilter]
    public async Task<IActionResult> Post([FromForm] MovieFormDto model, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR
                .Send(model.ConvertToCreateMovieCommand(model, _webHostEnvironment.WebRootPath,
                $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}")
                , cancellationToken);

            if (!res.Succeeded)
                return BadRequest(res.Errors);

            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpPut, Route("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ModelStateValidationFilter]
    public async Task<IActionResult> Put(int id, [FromForm] EditMovieFormDto model, CancellationToken cancellationToken)
    {
        try
        {
            model.Id = id;

            var res = await _mediatR.Send(model.ConvertToCreateMovieCommand(model, _webHostEnvironment.WebRootPath,
                $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}"), cancellationToken);

            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new DeleteMovieCommand(id, _webHostEnvironment.WebRootPath), cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }
}

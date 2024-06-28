using Application.Movie.Commands.DeleteMovie;
using Application.Movie.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MoviesApi.Dtos;
using MoviesApi.FileStorageServices.Liara;
using MoviesApi.Filters;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : BaseController
{
    #region constructor
    private readonly IMediator _mediatR;
    private readonly ILiaraStorageService _storageService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public MoviesController(IMediator mediatR,
        ILiaraStorageService storageService,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _mediatR = mediatR;
        _storageService = storageService;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    #endregion

    [HttpGet, Route("MovieFormInformations")]
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
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        try
        {
            var movieData = await _mediatR.Send(new GetMovieByIdQuery(id), cancellationToken);
            return Ok(movieData.Data);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpGet, Route("GetMovieDetailsForEdit/{id:int}")]
    [OutputCache(PolicyName = "SingleRow")]
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

    [HttpPost(Name = "CreateNewMovie")]
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
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new DeleteMovieCommand(id,_webHostEnvironment.WebRootPath),cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }
}

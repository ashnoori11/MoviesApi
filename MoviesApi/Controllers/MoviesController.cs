using Application.Movie.Commands.CreateMovie;
using Application.Movie.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Dtos;
using MoviesApi.FileStorageServices.Liara;

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
        IWebHostEnvironment  webHostEnvironment)
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

    [HttpPost(Name ="CreateNewMovie")]
    public async Task<IActionResult> Post([FromForm] MovieFormDto model,CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList());

            //string fileUrl = string.Empty;
            //if(model.Poster is object)
            //{
            //    var uploadImageResult = await _storageService.UploadToS3Async(model.Poster,cancellationToken);
            //    if (!uploadImageResult.Status)
            //        throw new Exception($"an error occared during upload the image - error : {uploadImageResult.Message}");

            //    fileUrl = uploadImageResult.FileName;
            //}

            var res = await _mediatR.Send(
                new CreateMovieCommand(
                    model.Title,
                    model.Summery,
                    model.Trailer,
                    model.InTheaters,
                    model.ReleaseDate,
                    model.Poster,
                    model.GenreIds,
                    model.MovieTheaterIds,
                    model.Actors,
                    _webHostEnvironment.WebRootPath,
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
}

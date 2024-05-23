using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieTheatersController : BaseController
{
    #region constructor
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMediator _mediatR;
    public MovieTheatersController(IMediator mediatR, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _mediatR = mediatR;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    #endregion
}

using Application.Actor.Commands.CreateActor;
using Application.Actor.Commands.DeleteActor;
using Application.Actor.Commands.UpdateActor;
using Application.Actor.Queries;
using Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : BaseController
{
    #region constructor
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMediator _mediatR;
    public ActorsController(IMediator mediatR, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
    {
        _mediatR = mediatR;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    #endregion

    [HttpGet(Name = "GetActors")]
    [OutputCache(PolicyName = "Paginating")]
    public async Task<IActionResult> Get([FromQuery] PaginationDto pagingDto, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediatR.Send(new GetAllActorsWithPaginationQuery(pagingDto.PageNumber, pagingDto.PageSize), cancellationToken);
            return Ok(result);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }


    [HttpGet("{actorId:int}",Name ="GetActorById")]
    public async Task<IActionResult> Get(int actorId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediatR.Send(new GetActorByIdQuery(actorId), cancellationToken);
            return Ok(result);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }


    [HttpPut("{actorId:int}",Name ="UpdateActor")]
    public async Task<IActionResult> Put(int actorId, [FromForm] CreateActorDto actor, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList());

        try
        {
            var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var res = await _mediatR.Send(new UpdateActorCommand(actorId, actor.Name, actor.DateOfBirth, actor.Biography, _webHostEnvironment.WebRootPath,actor.Picture, url), cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }


    [HttpPost(Name = "CreateActor")]
    public async Task<IActionResult> Post([FromForm] CreateActorDto actor, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList());

        try
        {
            var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var getResult = await _mediatR.Send(new CreateActorCommand(actor.Name, actor.DateOfBirth, actor.Biography, actor.Picture,_webHostEnvironment.WebRootPath, url), cancellationToken);
            return Ok(getResult);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }


    [HttpDelete("{actorId:int}",Name ="DeleteActor")]
    public async Task<IActionResult> Delete(int actorId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediatR.Send(new DeleteActorCommand(actorId,_webHostEnvironment.WebRootPath), cancellationToken);

            if (result.IsNotFound)
                return NotFound($"can not find actor with id : {actorId}");

            return Ok(result);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }
}

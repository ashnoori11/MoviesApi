using Application.Dtos;
using Application.Genre.Commands.CreateGenre;
using Application.Genre.Commands.EditGenre;
using Application.Genre.Commands.RemoveGenre;
using Application.Genre.Queries;
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
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
public class GenresController : BaseController
{
    #region constructor
    private readonly IMediator _mediatR;
    public GenresController(IMediator mediatR)
    {
        _mediatR = mediatR;
    }
    #endregion

    [HttpGet(Name = "GetGenres")]
    [OutputCache(PolicyName = "Paginating")]
    public async Task<IActionResult> Get([FromQuery] PaginationDto pagingDto, CancellationToken cancellationToken)
    {
        try
        {
            var getGenres = await _mediatR.Send(pagingDto.Adapt<GetAllGenresWithPaginationQuery>(), cancellationToken);
            return Ok(getGenres);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }

    [HttpGet("{genreId:int}")]
    [OutputCache(PolicyName = "SingleRow")]
    public async Task<IActionResult> Get(int genreId, CancellationToken cancellationToken)
    {
        try
        {
            var getGenres = await _mediatR.Send(new GetGenreByIdQuery(genreId), cancellationToken);
            return Ok(getGenres);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }

    [HttpGet,Route("All")]
    [AllowAnonymous]
    [OutputCache(PolicyName = "DropDowns")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new GetAllGenresQuery(),cancellationToken);
            return Ok(res.Data);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpPut("{genreId:int}")]
    [ModelStateValidationFilter]
    public async Task<IActionResult> Put(int genreId, [FromBody]CreateGenreDto genre,CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new EditGenreCommand(genreId,genre.Name),cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }

    [HttpPost(Name = "CreateGenre")]
    [ModelStateValidationFilter]
    public async Task<IActionResult> Post([FromBody] CreateGenreDto createGenre, CancellationToken cancellationToken)
    {
        try
        {
            var getResult = await _mediatR.Send(createGenre.Adapt<CreateGenreCommand>(), cancellationToken);
            return Ok(getResult);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediatR.Send(new RemoveGenreCommand(id),cancellationToken);

            if (result.IsNotFound)
                return NotFound($"can not find genre with id : {id}");

            return Ok(result);
        }
        catch (Exception exp)
        {
            return BadRequest(exp.Message);
        }
    }
}

using Application.Authentication.Commands.CreateUser;
using Application.Authentication.Commands.LoginUser;
using Application.Authentication.Commands.RemoveAdminRole;
using Application.Authentication.Commands.SetAdminRole;
using Application.Authentication.Queries;
using Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Filters;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(IMediator mediatR) : BaseController
{
    private readonly IMediator _mediatR = mediatR;

    [HttpPost("Create")]
    [AllowAnonymous]
    [ModelStateValidationFilter]
    public async Task<IActionResult> Create([FromBody] UserCredentialsDto model, CancellationToken cancellationToken)
    {
        try
        {
            var res = await mediatR.Send(new CreateAndLoginNewUserCommand(model.Email, model.Password), cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ModelStateValidationFilter]
    public async Task<IActionResult> Login([FromBody] UserCredentialsDto model,CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new LoginUserCommand(model.Email,model.Password),cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpGet,Route("UsersList")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
    public async Task<IActionResult> Get([FromQuery] PaginationDto paging, CancellationToken cancellationToken) 
    {
        try
        {
            var res = await _mediatR.Send(new GetUsersListQuery { PageNumber = paging .PageNumber,PageSize = paging .PageSize}
            , cancellationToken);

            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpPost,Route("MakeAdmin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
    public async Task<IActionResult> Post([FromBody] string userId,CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new SetAdminRoleCommand(userId), cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpPost, Route("RemoveAdminClaim")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
    public async Task<IActionResult> RemoveAdminClaim([FromBody] string userId, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _mediatR.Send(new RemoveAdminRoleCommand(userId), cancellationToken);
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }
}

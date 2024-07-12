using Application.Authentication.Commands.CreateUser;
using Application.Authentication.Commands.LoginUser;
using Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Filters;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(IMediator mediatR) : BaseController
{
    private readonly IMediator _mediatR = mediatR;

    [HttpPost("Create")]
    [ModelStateValidationFilter]
    public async Task<IActionResult> Create([FromBody] UserCredentialsDto model, CancellationToken cancellationToken)
    {
        try
        {
            var res = await mediatR.Send(new CreateAndLoginNewUserCommand(model.Email, model.Password));
            return Ok(res);
        }
        catch (Exception exp)
        {
            return BadRequest(exp);
        }
    }

    [HttpPost("LogIn")]
    [ModelStateValidationFilter]
    public async Task<IActionResult> LogIn([FromBody] UserCredentialsDto model,CancellationToken cancellationToken)
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
}

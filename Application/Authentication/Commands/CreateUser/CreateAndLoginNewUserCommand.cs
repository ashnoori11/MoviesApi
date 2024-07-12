using Application.Common.Models;
using Application.Services.IdentityServices.Contracts;
using Application.Services.JwtTokenService.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.CreateUser;

public record CreateAndLoginNewUserCommand(string Email, string Password) : IRequest<AuthenticationResponse>;

public class CreateAndLoginNewUserCommandHandler(IIdentityFactory identityFactory, IJwtTokenService jwtTokenService) : IRequestHandler<CreateAndLoginNewUserCommand, AuthenticationResponse>
{
    private readonly IIdentityFactory _identityFactory = identityFactory;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    public async Task<AuthenticationResponse> Handle(CreateAndLoginNewUserCommand request, CancellationToken cancellationToken)
    {
        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var res = await _identityFactory.CreateUserManager().CreateAsync(user, request.Password);

        if (res.Succeeded)
        {
            var tokenResult = await _jwtTokenService
                .GenerateTokenAsync(request.Email, cancellationToken);

            return AuthenticationResponse.Succeeded(tokenResult.Token, tokenResult.Expiration);
        }
        else
        {
            return AuthenticationResponse.Failed(res.Errors.Select(a => a.Description).ToArray());
        }
    }
}
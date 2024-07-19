using Application.Common.Models;
using Application.Services.IdentityServices.Contracts;
using Application.Services.JwtTokenService.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<AuthenticationResponse>;

public class LoginUserCommandHandler(IIdentityFactory identityFactory, IJwtTokenService jwtTokenService) : IRequestHandler<LoginUserCommand, AuthenticationResponse>
{
    private readonly IIdentityFactory _identityFactory = identityFactory;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    public async Task<AuthenticationResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityFactory
            .CreateSignInManager()
            .PasswordSignInAsync(request.Email,request.Password,
            isPersistent:false,lockoutOnFailure:false);

        if (result.Succeeded)
        {
            var _userManager = _identityFactory.CreateUserManager();
            var getUser = await _userManager.FindByEmailAsync(request.Email);
            var userClaims = await _userManager.GetClaimsAsync(getUser);

            bool isAdmin = false;

            if(userClaims.Any(a=>a.Value == "admin"))
            {
                isAdmin = true;
            }

            var tokenResult = await _jwtTokenService
                .GenerateTokenAsync(request.Email,cancellationToken, isAdmin);

            return AuthenticationResponse
                .Succeeded(tokenResult.Token, tokenResult.Expiration);
        }
        else
        {
            return AuthenticationResponse.Failed(new string[] { "The information entered is not correct !" });
        }
    }
}
using Application.Common.Models;
using Application.Services.IdentityServices.Contracts;
using MediatR;

namespace Application.Authentication.Commands.SetAdminRole;

public record SetAdminRoleCommand(string UserId) : IRequest<Result>;

public class SetAdminRoleCommandHandler(IIdentityFactory identityFactory) : IRequestHandler<SetAdminRoleCommand, Result>
{
    private readonly IIdentityFactory _identityFactory = identityFactory;

    public async Task<Result> Handle(SetAdminRoleCommand request, CancellationToken cancellationToken)
    {
        var _userManager = _identityFactory.CreateUserManager();

        var user = await _userManager
            .FindByIdAsync(userId: request.UserId);

        if (user is null)
            return Result.NotFound();

        await _userManager.AddClaimAsync(user,new System.Security.Claims.Claim("role","admin"));
        return Result.Success(); 
    }
}

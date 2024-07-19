using Application.Common.Models;
using Application.Services.IdentityServices.Contracts;
using MediatR;

namespace Application.Authentication.Commands.RemoveAdminRole;

public record RemoveAdminRoleCommand(string UserId) : IRequest<Result>;
public class RemoveAdminRoleCommandHandler(IIdentityFactory identityFactory) : IRequestHandler<RemoveAdminRoleCommand, Result>
{
    private readonly IIdentityFactory _identityFactory = identityFactory;
    public async Task<Result> Handle(RemoveAdminRoleCommand request, CancellationToken cancellationToken)
    {
        var _userManager = _identityFactory.CreateUserManager();

        var user = await _userManager.FindByIdAsync(userId: request.UserId);
        if (user is null)
            return Result.NotFound();

        await _userManager.RemoveClaimAsync(user, new System.Security.Claims.Claim("role", "admin"));
        return Result.Success();
    }
}
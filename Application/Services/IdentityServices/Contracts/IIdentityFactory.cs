using Microsoft.AspNetCore.Identity;

namespace Application.Services.IdentityServices.Contracts;

public interface IIdentityFactory
{
    UserManager<IdentityUser> CreateUserManager();
    SignInManager<IdentityUser> CreateSignInManager();
    RoleManager<IdentityRole> CreateRoleManager();
}

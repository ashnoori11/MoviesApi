using Application.Services.IdentityServices.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.IdentityServices;

public class IdentityFactory : IIdentityFactory
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityFactory(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public UserManager<IdentityUser> CreateUserManager() => _userManager;
    public SignInManager<IdentityUser> CreateSignInManager() => _signInManager;
    public RoleManager<IdentityRole> CreateRoleManager() => _roleManager;
}

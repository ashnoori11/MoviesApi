using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MoviesApi.Controllers;

public class BaseController : ControllerBase
{
	#region claims
	private string? _email;
    public string? CurrentUserEmail 
    {
        get
        {
            if (User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrWhiteSpace(_email))
                {
                    _email = User.FindFirstValue("email");
                }

                return _email;
            }
            else
            {
                return null;
            }
        }
    }
    #endregion
}

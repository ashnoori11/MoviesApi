using Microsoft.AspNetCore.Identity;

namespace Application.Common.Models;

public class AuthenticationResponse
{
    private AuthenticationResponse(string Token, DateTime Expiration)
    {
        this.Token = Token;
        this.Expiration = Expiration;
    }

    private AuthenticationResponse(string[] Errors)
    {
        Token = string.Empty;
        Expiration = DateTime.MinValue;
        this.Errors = Errors;
    }

    public string Token { get; private set; }
    public DateTime Expiration { get; private set; }
    public string[] Errors { get; set; }

    public static AuthenticationResponse Succeeded(string Token, DateTime Expiration)
        => new AuthenticationResponse(Token, Expiration);

    public static AuthenticationResponse Failed(string[] errors)
        => new AuthenticationResponse(errors);
}

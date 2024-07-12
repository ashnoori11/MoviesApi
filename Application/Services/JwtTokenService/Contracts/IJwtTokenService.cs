namespace Application.Services.JwtTokenService.Contracts;

public interface IJwtTokenService
{
    Task<(string Token, DateTime Expiration)> GenerateTokenAsync(string userName,CancellationToken cancellationToken);
}

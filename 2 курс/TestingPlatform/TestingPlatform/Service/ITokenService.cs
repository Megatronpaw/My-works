using TestingPlatform.Responses.Auth;

namespace TestingPlatform.Services;

public interface ITokenService
{
    string CreateAccessToken(AuthResponse authResponse);

    string CreateRefreshToken();
}
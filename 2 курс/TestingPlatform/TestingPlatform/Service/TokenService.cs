using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestingPlatform.Responses.Auth;
using TestingPlatform.Services;
using TestingPlatform.Settings;

namespace practice.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _settings;
    private readonly byte[] _key;

    public TokenService(IOptions<JwtSettings> opts)
    {
        _settings = opts.Value;
        _key = Encoding.UTF8.GetBytes(_settings.Key);
    }

    public string CreateAccessToken(AuthResponse authResponse)
    {
        var claims = new List<Claim>
       {
           new Claim(ClaimTypes.NameIdentifier, authResponse.Id.ToString()),
           new Claim(ClaimTypes.Name, authResponse.Login),
           new Claim(ClaimTypes.Email, authResponse.Email ?? string.Empty),
           new Claim(ClaimTypes.Role, authResponse.Role.ToString())
       };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_settings.AccessTokenMinutes);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Requests.Auth;
using TestingPlatform.Responses.Auth;
using TestingPlatform.Services;
using TestingPlatform.Settings;

namespace TestingPlatform.Controllers;

//TODO: практика
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class AuthController(IAuthRepository authRepository, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository, IMapper mapper, IOptions<JwtSettings> options) : ControllerBase
{
    private async Task GenerateAndSetRefreshTokenAsync(int userId)
    {
        var refreshToken = tokenService.CreateRefreshToken();
        var expires = DateTime.UtcNow.AddDays(options.Value.RefreshTokenDays);
        await refreshTokenRepository.SaveRefreshTokenAsync(userId, refreshToken, expires);

        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expires
        });
    }

    [HttpPost]
    public async Task<IActionResult> Authorize([FromBody] AuthRequest auth)
    {
        var userLoginDto = mapper.Map<UserLoginDto>(auth);
        var user = await authRepository.AuthorizeUser(userLoginDto);
        var response = mapper.Map<AuthResponse>(user);

        await GenerateAndSetRefreshTokenAsync(user.Id);

        var accessToken = tokenService.CreateAccessToken(response);

        return Ok(new
        {
            AccessToken = accessToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            return Unauthorized("Нет refresh токена");

        var refreshTokenDto = await refreshTokenRepository.RevokeTokenAsync(refreshToken);

        if (refreshTokenDto.Expires < DateTime.Now)
            return Unauthorized();

        var authResponse = mapper.Map<AuthResponse>(refreshTokenDto.User);
        await GenerateAndSetRefreshTokenAsync(refreshTokenDto.User.Id);

        var accessToken = tokenService.CreateAccessToken(authResponse);

        return Ok(new
        {
            AccessToken = accessToken
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            return Ok();

        await refreshTokenRepository.RevokeTokenAsync(refreshToken);
        Response.Cookies.Delete("refreshToken");
        return Ok();
    }
}



using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task SaveRefreshTokenAsync(int userId, string tokenRaw, DateTime expiresAt);
    Task<RefreshTokenDto> RevokeTokenAsync(string tokenRaw);
}
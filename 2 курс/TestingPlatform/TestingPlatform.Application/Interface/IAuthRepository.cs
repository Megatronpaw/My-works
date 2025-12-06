using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface IAuthRepository
{
    /// <summary>
    /// Аутентификация пользователя
    /// </summary>
    /// <param name="userLoginDto"></param>
    /// <returns></returns>
    Task<UserDto> AuthorizeUser(UserLoginDto userLoginDto);
}
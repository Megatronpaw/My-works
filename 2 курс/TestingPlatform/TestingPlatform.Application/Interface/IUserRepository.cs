using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <returns>Массив пользователей.</returns>
    Task<IEnumerable<UserDto>> GetAllAsync();

    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Пользователь.</returns>
    Task<UserDto> GetByIdAsync(int id);

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="userDto">Модель регистрации нового пользователя.</param>
    /// <returns>Идентификатор нового пользователя.</returns>
    Task<int> CreateAsync(UserDto userDto);

    /// <summary>
    /// Обновить информацию о пользователе.
    /// </summary>
    /// <param name="userDto">Модель обновления пользователя.</param>
    Task UpdateAsync(UserDto userDto);

    /// <summary>
    /// Удалить пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    Task DeleteAsync(int id);
}


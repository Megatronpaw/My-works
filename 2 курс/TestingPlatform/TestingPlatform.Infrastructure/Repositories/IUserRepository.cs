using TestingPlatform.Models;

namespace TestingPlatform.Application.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <returns>Массив пользователей.</returns>
    List<User> GetAllAsync();

    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Пользователь.</returns>
    User GetByIdAsync(int id);

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="user">Модель регистрации нового пользователя.</param>
    /// <returns>Идентификатор нового пользователя.</returns>
    int CreateAsync(User user);

    /// <summary>
    /// Обновить информацию о пользователе.
    /// </summary>
    /// <param name="user">Модель обновления пользователя.</param>
    void UpdateAsync(User user);

    /// <summary>
    /// Удалить пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    void DeleteAsync(int id);
}
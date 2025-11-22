using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface IAttemptRepository
{
    /// <summary>
    /// Создать новую попытку
    /// </summary>
    /// <param name="attemptDto">Модель создания новой попытки</param>
    /// <returns>Идентификатор новой попытки</returns>
    Task<int> CreateAsync(AttemptDto attemptDto);

    /// <summary>
    /// Обновить попытку
    /// </summary>
    /// <param name="attemptDto">Модель попытки</param>
    /// <returns></returns>
    Task UpdateAsync(AttemptDto attemptDto);
}


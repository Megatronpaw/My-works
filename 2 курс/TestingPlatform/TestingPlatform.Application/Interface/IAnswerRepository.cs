using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface IAnswerRepository
{
    /// <summary>
    /// Получить список ответов
    /// </summary>
    /// <returns>Список ответов</returns>
    Task<IEnumerable<AnswerDto>> GetAllAsync();

    /// <summary>
    /// Получить ответ по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор ответа</param>
    /// <returns>Ответ</returns>
    Task<AnswerDto> GetByIdAsync(int id);

    /// <summary>
    /// Добавить новый ответ
    /// </summary>
    /// <param name="answerDto">Модель создания нового ответа</param>
    /// <returns>Идентификатор нового ответа</returns>
    Task<int> CreateAsync(AnswerDto answerDto);

    /// <summary>
    /// Обновить информацию об ответе
    /// </summary>
    /// <param name="answerDto">Модель обновления ответа</param>
    Task UpdateAsync(AnswerDto answerDto);

    /// <summary>
    /// Удалить ответ
    /// </summary>
    /// <param name="id">Идентификатор ответа</param>
    Task DeleteAsync(int id);
}
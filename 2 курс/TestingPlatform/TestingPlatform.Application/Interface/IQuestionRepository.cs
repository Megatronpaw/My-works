using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface IQuestionRepository
{
    /// <summary>
    /// Получить все вопросы.
    /// </summary>
    /// <returns>Массив вопросов.</returns>
    Task<IEnumerable<QuestionDto>> GetAllAsync();

    /// <summary>
    /// Получить вопрос по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор вопроса.</param>
    /// <returns>Вопрос.</returns>
    Task<QuestionDto> GetByIdAsync(int id);

    /// <summary>
    /// Создать новый вопрос.
    /// </summary>
    /// <param name="questionDto">Модель создания нового вопроса.</param>
    /// <returns>Идентификатор нового вопроса.</returns>
    Task<int> CreateAsync(QuestionDto questionDto);

    /// <summary>
    /// Обновить информацию о вопросе.
    /// </summary>
    /// <param name="questionDto">Модель обновления вопроса.</param>
    Task UpdateAsync(QuestionDto questionDto);

    /// <summary>
    /// Удалить вопрос.
    /// </summary>
    /// <param name="id">Идентификатор вопроса.</param>
    Task DeleteAsync(int id);
}


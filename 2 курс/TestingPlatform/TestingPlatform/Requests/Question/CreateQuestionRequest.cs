using TestingPlatform.Enums;

namespace TestingPlatform.Requests.Question;

/// <summary>
/// Модель создания вопроса
/// </summary>
public class CreateQuestionRequest
{
    /// <summary>
    /// Текст вопросы
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Номер вопроса в тесте
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Описание вопроса
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Тип ответа на вопрос
    /// </summary>
    public AnswerType AnswerType { get; set; }

    /// <summary>
    /// Оценивается ли вопрос
    /// </summary>
    public bool IsScoring { get; set; }

    /// <summary>
    /// Балл за вопрос (если оценивается)
    /// </summary>
    public int? MaxScore { get; set; }

    /// <summary>
    /// Идентификатор теста
    /// </summary>
    public int TestId { get; set; }
}


using TestingPlatform.Enums;

namespace TestingPlatform.Responses.Question;

/// <summary>
/// Вопрос
/// </summary>
public class QuestionResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Тест вопроса
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
    /// Оценивается ли тест
    /// </summary>
    public bool IsScoring { get; set; }

    /// <summary>
    /// Сколько баллов можно получить за вопрос
    /// </summary>
    public int MaxScore { get; set; }

    // TODO: в будущем добавим получение вариантов ответов на вопрос
}


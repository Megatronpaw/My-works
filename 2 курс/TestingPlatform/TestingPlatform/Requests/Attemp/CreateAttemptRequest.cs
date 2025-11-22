namespace TestingPlatform.Requests.Attempt;

/// <summary>
/// Модель начала попытки
/// </summary>
public class CreateAttemptRequest
{
    /// <summary>
    /// Идентификатор теста
    /// </summary>
    public int TestId { get; set; }

    //TODO: удалить после добавления авторизации
    /// <summary>
    /// [Временно (после добавления авторизации будет удален)] Идентификатор студента
    /// </summary>
    public int StudentId { get; set; }
}


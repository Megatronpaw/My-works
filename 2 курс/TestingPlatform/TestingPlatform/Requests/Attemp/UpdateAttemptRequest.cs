namespace TestingPlatform.Requests.Attempt;

public class UpdateAttemptRequest
{
    /// <summary>
    /// Идентфикатор попытки
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор студента
    /// </summary>
    public int StudentId { get; set; }
}


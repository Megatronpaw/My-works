using TestingPlatform.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestingPlatform.Application.Dtos;

public class AttemptDto
{
    /// <summary>
    /// Идентификатор попытки
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Время начала попытки
    /// </summary>
    public DateTimeOffset StartedAt { get; set; }

    /// <summary>
    /// Время окончания попытки
    /// </summary>
    public DateTimeOffset? SubmittedAt { get; set; }

    /// <summary>
    /// Количество баллов за попытку
    /// </summary>
    public int? Score { get; set; }

    /// <summary>
    /// По какому тесту была попытка
    /// </summary>
    public int TestId { get; set; }

    /// <summary>
    /// Какой студент выполнял попытку
    /// </summary>
    public int StudentId { get; set; }
}
using System.ComponentModel.DataAnnotations;
using TestingPlatform.Enums;

namespace TestingPlatform.Requests.Test;

public class CreateTestRequest
{
    [Required(ErrorMessage = "Название теста обязательно")]
    [StringLength(200, ErrorMessage = "Название не может превышать 200 символов")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Описание теста обязательно")]
    [StringLength(1000, ErrorMessage = "Описание не может превышать 1000 символов")]
    public string Description { get; set; }

    public bool IsRepeatable { get; set; } = false;

    [Required(ErrorMessage = "Тип теста обязателен")]
    public TestType Type { get; set; }

    [Required(ErrorMessage = "Дата публикации обязательна")]
    public DateTime PublishedAt { get; set; }

    [Required(ErrorMessage = "Дедлайн обязателен")]
    public DateTime Deadline { get; set; }

    [Range(1, 480, ErrorMessage = "Длительность должна быть от 1 до 480 минут")]
    public int? DurationMinutes { get; set; }

    public bool IsPublic { get; set; } = false;

    [Range(0, 1000, ErrorMessage = "Проходной балл должен быть от 0 до 1000")]
    public int? PassingScore { get; set; }

    [Range(1, 10, ErrorMessage = "Максимальное количество попыток должно быть от 1 до 10")]
    public int? MaxAttempts { get; set; }

    // Добавляем недостающие свойства
    public List<int> Students { get; set; } = new();
    public List<int> Projects { get; set; } = new();
    public List<int> Courses { get; set; } = new();
    public List<int> Groups { get; set; } = new();
    public List<int> Directions { get; set; } = new();
}
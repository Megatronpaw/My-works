using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Requests.Student;

/// <summary>
/// Модель создания студента
/// </summary>
public class CreateStudentRequest
{
    /// <summary>
    /// Логин
    /// </summary>
    [Required]
    public string Login { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    [Required]
    public string FirstName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public string MiddleName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    [Required]
    public string LastName { get; set; }

    /// <summary>
    /// Телефон
    /// </summary>
    [Required]
    public string Phone { get; set; }

    /// <summary>
    /// Ссылка на профиль VK
    /// </summary>
    [Required]
    public string VkProfileLink { get; set; }
}


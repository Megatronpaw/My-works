using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Requests.Answer;

public class UpdateAnswerRequest
{
    public int Id { get; set; }

    [Required]
    public string Text { get; set; }

    [Required]
    public bool IsCorrect { get; set; }

    [Required]
    public int QuestionId { get; set; }
}
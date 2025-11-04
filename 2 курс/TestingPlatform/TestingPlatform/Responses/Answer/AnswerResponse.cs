namespace TestingPlatform.Responses.Answer;

public class AnswerResponse
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
}
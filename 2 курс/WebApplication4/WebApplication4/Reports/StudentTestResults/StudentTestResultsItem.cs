public class StudentTestResultsItem
{
    public int TestId { get; set; }
    public string TestTitle { get; set; } = string.Empty;
    public int BestScore { get; set; }
    public int MaxPossibleScore { get; set; }
    public bool Passed { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int AttemptsCount { get; set; } // Сколько раз пытался пройти
}

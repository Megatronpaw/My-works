public class StudentPassRateSummaryItem
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int TestsAttempted { get; set; }    // Сколько тестов студент пытался пройти
    public int TestsPassed { get; set; }       // Сколько тестов пройдено успешно
    public decimal PassRate { get; set; }      // Процент (0-100)
    public int TotalScore { get; set; }        // Сумма всех баллов
    public decimal AverageScore { get; set; }  // Средний балл за попытку
}

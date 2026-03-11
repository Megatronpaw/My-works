public class GroupTrendItem
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public DateTime Month { get; set; }        // 2024-03-01
    public string MonthLabel { get; set; } = string.Empty; // "Мар 2024"
    public decimal AverageScore { get; set; }
    public int AttemptsCount { get; set; }
}

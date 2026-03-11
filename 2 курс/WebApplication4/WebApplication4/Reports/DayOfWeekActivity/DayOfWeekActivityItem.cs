public class DayOfWeekActivityItem
{
    public int DayOfWeek { get; set; } // 0=Sunday, 1=Monday, ... 6=Saturday
    public string DayName { get; set; } = string.Empty; // "Пн", "Вт", ...
    public int TestsCompleted { get; set; } // Количество завершённых попыток
    public int UniqueStudents { get; set; } // Уникальные студенты (опционально)
}

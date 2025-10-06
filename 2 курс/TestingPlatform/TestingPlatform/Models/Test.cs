using TestingPlatform.Controllers;
using TestingPlatform.Enums;

namespace TestingPlatform.Models;
public class Test
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRepeatable { get; set; } = false;
    public TestType Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset PublishedAt { get; set; }
    public DateTimeOffset Deadline { get; set; }
    public int? DurationMinutes { get; set; }
    public bool IsPublic { get; set; } = false;
    public int? PassingScore { get; set; }
    public int? MaxAttempts { get; set; }

    public List<Question> Questions { get; set; } = new();
    public List<Student> Students { get; set; } = new();
    public List<Project> Projects { get; set; } = new();
    public List<Course> Courses { get; set; } = new();
    public List<Group> Groups { get; set; } = new();
    public List<Direction> Directions { get; set; } = new();
}
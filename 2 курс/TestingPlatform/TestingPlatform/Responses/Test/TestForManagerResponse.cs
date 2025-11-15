using TestingPlatform.Enums;
using TestingPlatform.Responses.Course;
using TestingPlatform.Responses.Direction;
using TestingPlatform.Responses.Group;
using TestingPlatform.Responses.Project;
using TestingPlatform.Responses.Student;

namespace TestingPlatform.Responses.Test;

public class TestForManagerResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRepeatable { get; set; }
    public TestType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime Deadline { get; set; }
    public int? DurationMinutes { get; set; }
    public bool IsPublic { get; set; }
    public int? PassingScore { get; set; }
    public int? MaxAttempts { get; set; }

    public List<StudentForTestResponse> Students { get; set; } = new();
    public List<ProjectResponse> Projects { get; set; } = new();
    public List<CourseResponse> Courses { get; set; } = new();
    public List<GroupResponse> Groups { get; set; } = new();
    public List<DirectionResponse> Directions { get; set; } = new();
}
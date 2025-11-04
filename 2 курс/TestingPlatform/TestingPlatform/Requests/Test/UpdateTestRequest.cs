using System.ComponentModel.DataAnnotations;
using TestingPlatform.Enums;

namespace TestingPlatform.Requests.Test;

public class UpdateTestRequest
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    public bool IsRepeatable { get; set; }

    [Required]
    public TestType Type { get; set; }

    public DateTimeOffset PublishedAt { get; set; }

    public DateTimeOffset Deadline { get; set; }

    public int? DurationMinutes { get; set; }

    public bool IsPublic { get; set; }

    public int? PassingScore { get; set; }

    public int? MaxAttempts { get; set; }
}
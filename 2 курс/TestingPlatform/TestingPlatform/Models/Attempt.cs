namespace TestingPlatform.Models
{
    public class Attempt
    {
        public int Id { get; set; }
        public DateTimeOffset StartedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? SubmittedAt { get; set; }
        public int? Score { get; set; }
        public int TestId { get; set; }
        public int StudentId { get; set; }

        public Test Test { get; set; }
        public Student Student { get; set; }
        public List<UserAttemptAnswer> UserAttemptAnswers { get; set; } = new();
    }
}

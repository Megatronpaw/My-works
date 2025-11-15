namespace TestingPlatform.Models
{
    public class Attempt
    {
        public int Id { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAt { get; set; }
        public int? Score { get; set; }
        public int TestId { get; set; }
        public int StudentId { get; set; }

        public Test Test { get; set; }
        public Student Student { get; set; }
        public List<UserAttemptAnswer> UserAttemptAnswers { get; set; } = new();
    }
}
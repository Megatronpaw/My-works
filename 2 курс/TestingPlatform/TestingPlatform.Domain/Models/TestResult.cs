namespace TestingPlatform.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        public bool Passed { get; set; }
        public int TestId { get; set; }
        public int AttemptId { get; set; }
        public int StudentId { get; set; }

        public Test Test { get; set; }
        public Attempt Attempt { get; set; }
        public Student Student { get; set; }
    }
}

namespace TestingPlatform.Models
{
    public class UserSelectedOption
    {
        public int Id { get; set; }
        public int UserAttemptAnswerId { get; set; }
        public int AnswerId { get; set; }

        public UserAttemptAnswer UserAttemptAnswer { get; set; }
        public Answer Answer { get; set; }
    }
}

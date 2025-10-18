namespace TestingPlatform.Models
{
    public class UserTextAnswer
    {
        public int Id { get; set; }
        public string TextAnswer { get; set; }
        public int UserAttemptAnswerId { get; set; }

        public UserAttemptAnswer UserAttemptAnswer { get; set; }
    }
}

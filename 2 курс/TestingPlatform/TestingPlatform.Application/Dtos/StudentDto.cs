namespace TestingPlatform.Application.Dtos;

public class StudentDto
{
    public int Id { get; set; }
    public string Phone { get; set; }
    public string VkProfileLink { get; set; }
    public int UserId { get; set; }
    public UserDto User { get; set; }

    public int? EducationScore { get; set; }
    public int? AdditionalActivityScore { get; set; }
    public int? OtherScore { get; set; }
}
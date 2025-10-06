using Microsoft.EntityFrameworkCore;
using TestingPlatform.Models;
public class Student
{
    public int Id { get; set; }
    public string Phone { get; set; }
    public string VkProfileLink { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Group> Groups { get; set; } = new();
    public List<Test> Tests { get; set; } = new(); 
    public List<Attempt> Attempts { get; set; } = new();
    public List<TestResult> TestResults { get; set; } = new();

}


using Microsoft.EntityFrameworkCore;
using TestingPlatform.Models;
public class Student
{
    public int Id { get; set; }
    public string Phone { get; set; }
    public string VkProfileLink { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

}


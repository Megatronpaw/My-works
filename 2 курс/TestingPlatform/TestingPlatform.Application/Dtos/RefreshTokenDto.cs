using TestingPlatform.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestingPlatform.Application.Dtos;

public class RefreshTokenDto
{
    public UserDto User { get; set; }
    public DateTime Expires { get; set; }
}

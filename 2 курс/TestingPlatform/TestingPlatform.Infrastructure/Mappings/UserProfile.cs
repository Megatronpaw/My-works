using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
using AutoMapper;
using TestingPlatform.Responses.Auth;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Auth;

namespace practice.Mappings;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<AuthRequest, UserLoginDto>();
        CreateMap<UserDto, AuthResponse>();
    }
}
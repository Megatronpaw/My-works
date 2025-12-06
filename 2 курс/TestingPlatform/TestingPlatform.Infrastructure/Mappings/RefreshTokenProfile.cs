using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Domain.Models;

namespace TestingPlatform.Infrastructure.Mappings;

public class RefreshTokenProfile : Profile
{
    public RefreshTokenProfile()
    {
        CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
    }
}
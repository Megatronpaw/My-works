using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Mappings;

public class DirectionProfile : Profile
{
    public DirectionProfile()
    {
        CreateMap<Direction, DirectionDto>().ReverseMap();
    }
}
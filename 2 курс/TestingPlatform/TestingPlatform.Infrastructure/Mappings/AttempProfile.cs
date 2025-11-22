using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Mappings;

public class AttemptProfile : Profile
{
    public AttemptProfile()
    {
        CreateMap<AttemptDto, Attempt>();
    }
}
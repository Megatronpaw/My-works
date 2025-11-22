using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Attempt;

namespace TestingPlatform.Mappings;

public class AttemptProfile : Profile
{
    public AttemptProfile()
    {
        CreateMap<CreateAttemptRequest, AttemptDto>();
        CreateMap<UpdateAttemptRequest, AttemptDto>();
    }
}
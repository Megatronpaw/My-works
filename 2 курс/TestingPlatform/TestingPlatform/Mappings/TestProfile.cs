using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Test;
using TestingPlatform.Responses.Test;

namespace TestingPlatform.Mappings;

public class TestProfile : Profile
{
    public TestProfile()
    {
        CreateMap<TestDto, TestResponse>();
        CreateMap<CreateTestRequest, TestDto>();
        CreateMap<UpdateTestRequest, TestDto>();
    }
}
using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Answer;
using TestingPlatform.Responses.Answer;

namespace TestingPlatform.Mappings;

public class AnswerProfile : Profile
{
    public AnswerProfile()
    {
        CreateMap<AnswerDto, AnswerResponse>();
        CreateMap<CreateAnswerRequest, AnswerDto>();
        CreateMap<UpdateAnswerRequest, AnswerDto>();
    }
}
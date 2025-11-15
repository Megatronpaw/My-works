using AutoMapper;
using TestingPlatform.Requests.Question;
using TestingPlatform.Responses.Question;
using TestingPlatform.Application.Dtos;

namespace practice.Mappings;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<QuestionDto, QuestionResponse>();
        CreateMap<CreateQuestionRequest, QuestionDto>();
        CreateMap<UpdateQuestionRequest, QuestionDto>();
    }
}


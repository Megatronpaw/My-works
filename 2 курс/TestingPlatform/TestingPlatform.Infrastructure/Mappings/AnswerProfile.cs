using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Mappings;

public class AnswerProfile : Profile
{
    public AnswerProfile()
    {
        CreateMap<Answer, AnswerDto>().ReverseMap();
    }
}
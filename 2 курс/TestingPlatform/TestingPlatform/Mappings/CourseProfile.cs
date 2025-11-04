using AutoMapper;
using TestingPlatform.Responses.Course;
using TestingPlatform.Application.Dtos;

namespace practice.Mappings;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<CourseDto, CourseResponse>();
    }
}
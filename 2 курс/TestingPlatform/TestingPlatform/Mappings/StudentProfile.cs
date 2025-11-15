using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Student;
using TestingPlatform.Responses.Student;

namespace practice.Mappings;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<StudentDto, StudentForTestResponse>()
            .ForMember(d => d.FirstName, m => m.MapFrom(s => s.User.FirstName))
            .ForMember(d => d.MiddleName, m => m.MapFrom(s => s.User.MiddleName))
            .ForMember(d => d.LastName, m => m.MapFrom(s => s.User.LastName));
        CreateMap<StudentDto, StudentResponse>()
            .ForMember(d => d.Login, m => m.MapFrom(s => s.User.Login))
            .ForMember(d => d.Email, m => m.MapFrom(s => s.User.Email))
            .ForMember(d => d.FirstName, m => m.MapFrom(s => s.User.FirstName))
            .ForMember(d => d.MiddleName, m => m.MapFrom(s => s.User.MiddleName))
            .ForMember(d => d.LastName, m => m.MapFrom(s => s.User.LastName))
            .ForMember(d => d.Phone, m => m.MapFrom(s => s.Phone))
            .ForMember(d => d.VkProfileLink, m => m.MapFrom(s => s.VkProfileLink));
        CreateMap<UpdateStudentRequest, StudentDto>();

    }
}


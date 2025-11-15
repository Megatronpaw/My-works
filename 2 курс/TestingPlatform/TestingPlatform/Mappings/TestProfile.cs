using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Requests.Test;
using TestingPlatform.Responses.Test;

namespace practice.Mappings;

public class TestProfile : Profile
{
    public TestProfile()
    {
        CreateMap<TestDto, TestResponse>();
        CreateMap<TestDto, TestForManagerResponse>();
        CreateMap<TestDto, TestForStudentResponse>();
        CreateMap<CreateTestRequest, TestDto>()
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.Title, m => m.MapFrom(s => s.Title))
            .ForMember(d => d.Description, m => m.MapFrom(s => s.Description))
            .ForMember(d => d.IsRepeatable, m => m.MapFrom(s => s.IsRepeatable))
            .ForMember(d => d.Type, m => m.MapFrom(s => s.Type))
            .ForMember(d => d.PublishedAt, m => m.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.Deadline, m => m.MapFrom(s => s.Deadline))
            .ForMember(d => d.DurationMinutes, m => m.MapFrom(s => s.DurationMinutes))
            .ForMember(d => d.PassingScore, m => m.MapFrom(s => s.PassingScore))
            .ForMember(d => d.MaxAttempts, m => m.MapFrom(s => s.MaxAttempts))
            .ForMember(d => d.IsPublic, m => m.Ignore())
            .ForMember(d => d.Students, m => m.Ignore())
            .ForMember(d => d.Projects, m => m.Ignore())
            .ForMember(d => d.Courses, m => m.Ignore())
            .ForMember(d => d.Groups, m => m.Ignore())
            .ForMember(d => d.Directions, m => m.Ignore())
            // добавили маппинг для списков сущностей, для которых предназначены тесты
            .AfterMap((src, dest) =>
            {
                dest.Students = src.Students.Select(id => new StudentDto { Id = id }).ToList();
                dest.Projects = src.Projects.Select(id => new ProjectDto { Id = id }).ToList();
                dest.Courses = src.Courses.Select(id => new CourseDto { Id = id }).ToList();
                dest.Groups = src.Groups.Select(id => new GroupDto { Id = id }).ToList();
                dest.Directions = src.Directions.Select(id => new DirectionDto { Id = id }).ToList();
            });
        CreateMap<UpdateTestRequest, TestDto>()
            .ForMember(d => d.Id, m => m.MapFrom(s => s.Id))
            .ForMember(d => d.Title, m => m.MapFrom(s => s.Title))
            .ForMember(d => d.Description, m => m.MapFrom(s => s.Description))
            .ForMember(d => d.IsRepeatable, m => m.MapFrom(s => s.IsRepeatable))
            .ForMember(d => d.Type, m => m.MapFrom(s => s.Type))
            .ForMember(d => d.PublishedAt, m => m.MapFrom(s => s.PublishedAt))
            .ForMember(d => d.Deadline, m => m.MapFrom(s => s.Deadline))
            .ForMember(d => d.DurationMinutes, m => m.MapFrom(s => s.DurationMinutes))
            .ForMember(d => d.PassingScore, m => m.MapFrom(s => s.PassingScore))
            .ForMember(d => d.MaxAttempts, m => m.MapFrom(s => s.MaxAttempts))
            .ForMember(d => d.IsPublic, m => m.Ignore())
            .ForMember(d => d.Students, m => m.Ignore())
            .ForMember(d => d.Projects, m => m.Ignore())
            .ForMember(d => d.Courses, m => m.Ignore())
            .ForMember(d => d.Groups, m => m.Ignore())
            .ForMember(d => d.Directions, m => m.Ignore())
            // добавили маппинг для списков сущностей, для которых предназначены тесты
            .AfterMap((src, dest) =>
            {
                dest.Students = src.Students.Select(id => new StudentDto { Id = id }).ToList();
                dest.Projects = src.Projects.Select(id => new ProjectDto { Id = id }).ToList();
                dest.Courses = src.Courses.Select(id => new CourseDto { Id = id }).ToList();
                dest.Groups = src.Groups.Select(id => new GroupDto { Id = id }).ToList();
                dest.Directions = src.Directions.Select(id => new DirectionDto { Id = id }).ToList();
            });
    }
}


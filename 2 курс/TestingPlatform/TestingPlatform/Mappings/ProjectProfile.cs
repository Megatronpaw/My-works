using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Responses.Project;

namespace practice.Mappings;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectDto, ProjectResponse>();
    }
}
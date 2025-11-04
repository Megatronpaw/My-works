using AutoMapper;
using TestingPlatform.Responses.Group;
using TestingPlatform.Application.Dtos;

namespace practice.Mappings;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<GroupDto, GroupResponse>();
        CreateMap<CreateGroupRequest, GroupDto>()
            .ForMember(d => d.Direction, o => o.MapFrom(s => s.DirectionId > 0 ? new DirectionDto { Id = s.DirectionId } : null))
            .ForMember(d => d.Course, o => o.MapFrom(s => s.CourseId > 0 ? new CourseDto { Id = s.CourseId } : null))
            .ForMember(d => d.Project, o => o.MapFrom(s => s.ProjectId > 0 ? new ProjectDto { Id = s.ProjectId } : null));
        CreateMap<UpdateGroupRequest, GroupDto>()
            .ForMember(d => d.Direction, o => o.MapFrom(s => s.DirectionId > 0 ? new DirectionDto { Id = s.DirectionId } : null))
            .ForMember(d => d.Course, o => o.MapFrom(s => s.CourseId > 0 ? new CourseDto { Id = s.CourseId } : null))
            .ForMember(d => d.Project, o => o.MapFrom(s => s.ProjectId > 0 ? new ProjectDto { Id = s.ProjectId } : null));
    }
}


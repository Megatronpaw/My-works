using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Repositories;

public class GroupRepository(AppDbContext appDbContext, IMapper mapper) : IGroupRepository
{
    public List<GroupDto> GetAll()
    {
        var groups = appDbContext.Groups
            .Include(g => g.Project)
            .Include(g => g.Direction)
            .Include(g => g.Course)
            .Include(g => g.Students)
            .AsNoTracking()
            .ToList();

        return mapper.Map<List<GroupDto>>(groups);
    }

    public GroupDto GetById(int id)
    {
        var group = appDbContext.Groups
            .Include(g => g.Project)
            .Include(g => g.Course)
            .Include(g => g.Direction)
            .AsNoTracking()
            .FirstOrDefault(group => group.Id == id);

        if (group == null)
        {
            throw new Exception("Группа не найдена.");
        }

        return mapper.Map<GroupDto>(group);
    }

    public int Create(GroupDto groupDto)
    {
        var group = mapper.Map<Group>(groupDto);

        var direction = appDbContext.Directions.FirstOrDefault(d => d.Id == groupDto.Direction.Id);
        if (direction is null)
        {
            throw new Exception("Направление не найдено.");
        }
        group.Direction = direction;

        var course = appDbContext.Courses.FirstOrDefault(c => c.Id == groupDto.Course.Id);
        if (course is null)
        {
            throw new Exception("Курс не найден.");
        }
        group.Course = course;

        var project = appDbContext.Projects.FirstOrDefault(p => p.Id == groupDto.Project.Id);
        if (project is null)
        {
            throw new Exception("Проект не найден.");
        }
        group.Project = project;

        var groupId = appDbContext.Add(group);
        appDbContext.SaveChanges();

        return groupId.Entity.Id;
    }

    public void Update(GroupDto groupDto)
    {
        var group = appDbContext.Groups.FirstOrDefault(group => group.Id == groupDto.Id);

        if (group == null)
        {
            throw new Exception("Группа не найдена.");
        }

        group.Name = groupDto.Name;
        group.CourseId = groupDto.Course.Id;
        group.DirectionId = groupDto.Direction.Id;
        group.ProjectId = groupDto.Project.Id;

        appDbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var group = appDbContext.Groups.FirstOrDefault(group => group.Id == id);

        if (group == null)
        {
            throw new Exception("Группа не найдена.");
        }

        appDbContext.Groups.Remove(group);
        appDbContext.SaveChanges();
    }
}
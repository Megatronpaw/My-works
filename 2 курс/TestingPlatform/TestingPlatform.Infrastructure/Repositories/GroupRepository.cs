using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure;
using TestingPlatform.Infrastructure.Exceptions;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GroupRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<GroupDto>> GetAllAsync()
    {
        var groups = await _context.Groups
            .Include(g => g.Project)
            .Include(g => g.Direction)
            .Include(g => g.Course)
            .Include(g => g.Students)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<GroupDto>>(groups);
    }

    public async Task<GroupDto> GetByIdAsync(int id)
    {
        var group = await _context.Groups
            .Include(g => g.Project)
            .Include(g => g.Course)
            .Include(g => g.Direction)
            .AsNoTracking()
            .FirstOrDefaultAsync(group => group.Id == id);

        if (group == null)
        {
            throw new EntityNotFoundException("Группа", id);
        }

        return _mapper.Map<GroupDto>(group);
    }

    public async Task<int> CreateAsync(GroupDto groupDto)
    {
        var group = _mapper.Map<Group>(groupDto);

        var direction = await _context.Directions.FirstOrDefaultAsync(d => d.Id == groupDto.Direction.Id);
        if (direction is null)
        {
            throw new EntityNotFoundException("Направление", groupDto.Direction.Id);
        }
        group.Direction = direction;

        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == groupDto.Course.Id);
        if (course is null)
        {
            throw new EntityNotFoundException("Курс", groupDto.Course.Id);
        }
        group.Course = course;

        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == groupDto.Project.Id);
        if (project is null)
        {
            throw new EntityNotFoundException("Проект", groupDto.Project.Id);
        }
        group.Project = project;

        var groupId = await _context.AddAsync(group);
        await _context.SaveChangesAsync();

        return groupId.Entity.Id;
    }

    public async Task UpdateAsync(GroupDto groupDto)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(group => group.Id == groupDto.Id);

        if (group == null)
        {
            throw new EntityNotFoundException("Группа", groupDto.Id);
        }

        group.Name = groupDto.Name;
        group.CourseId = groupDto.Course.Id;
        group.DirectionId = groupDto.Direction.Id;
        group.ProjectId = groupDto.Project.Id;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(group => group.Id == id);

        if (group == null)
        {
            throw new EntityNotFoundException("Группа", id);
        }

        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();
    }
}
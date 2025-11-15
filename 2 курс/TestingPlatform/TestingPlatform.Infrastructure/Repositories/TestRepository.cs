using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure;
using TestingPlatform.Infrastructure.Exceptions;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Repositories;

public class TestRepository : ITestRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TestRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TestDto>> GetAllAsync(bool? isPublic, List<int> groupIds, List<int> studentIds)
    {
        await RefreshPublicationStatusesAsync();

        var tests = _context.Tests
            .OrderByDescending(t => t.PublishedAt)
            .ThenBy(t => t.Title)
            .AsNoTracking()
            .AsQueryable();

        if (isPublic is not null)
            tests = tests.Where(t => t.IsPublic == isPublic);

        if (groupIds != null && groupIds.Any())
            tests = tests.Where(t => t.Groups.Any(g => groupIds.Contains(g.Id)));

        if (studentIds != null && studentIds.Any())
            tests = tests.Where(t => t.Students.Any(s => studentIds.Contains(s.Id)));

        return _mapper.Map<IEnumerable<TestDto>>(await tests.ToListAsync());
    }

    public async Task<IEnumerable<TestDto>> GetAllForStudent(int studentId)
    {
        await RefreshPublicationStatusesAsync();

        var tests = await _context.Tests
            .Where(t => t.IsPublic)
            .Where(t =>
                t.Students.Any(s => s.Id == studentId) ||
                t.Courses.Any(c => c.Groups.Any(g => g.Students.Any(s => s.Id == studentId))) ||
                t.Projects.Any(p => p.Groups.Any(g => g.Students.Any(s => s.Id == studentId))) ||
                t.Directions.Any(d => d.Groups.Any(g => g.Students.Any(s => s.Id == studentId)))
            )
            .ToListAsync();

        return _mapper.Map<IEnumerable<TestDto>>(tests);
    }

    public async Task<TestDto> GetByIdAsync(int id)
    {
        await RefreshPublicationStatusesAsync();

        var test = await _context.Tests
            .Include(test => test.Directions)
            .Include(test => test.Courses)
            .Include(test => test.Groups)
            .Include(test => test.Projects)
            .Include(test => test.Students)
                .ThenInclude(student => student.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(test => test.Id == id);

        if (test == null)
        {
            throw new EntityNotFoundException("Тест", id);
        }

        return _mapper.Map<TestDto>(test);
    }

    public async Task<int> CreateAsync(TestDto testDto)
    {
        var test = _mapper.Map<Test>(testDto);
        var testId = await _context.Tests.AddAsync(test);
        await UpdateMembersTest(test, testDto);
        await _context.SaveChangesAsync();
        return testId.Entity.Id;
    }

    public async Task UpdateAsync(TestDto testDto)
    {
        var test = await _context.Tests.FirstOrDefaultAsync(test => test.Id == testDto.Id);

        if (test == null)
        {
            throw new EntityNotFoundException("Тест", testDto.Id);
        }

        test.Title = testDto.Title;
        test.Description = testDto.Description;
        test.IsRepeatable = testDto.IsRepeatable;
        test.Type = testDto.Type;
        test.PublishedAt = testDto.PublishedAt;
        test.Deadline = testDto.Deadline;
        test.DurationMinutes = testDto.DurationMinutes;
        test.IsPublic = testDto.IsPublic;
        test.PassingScore = testDto.PassingScore;
        test.MaxAttempts = testDto.MaxAttempts;

        await UpdateMembersTest(test, testDto);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var test = await _context.Tests.FirstOrDefaultAsync(test => test.Id == id);

        if (test == null)
        {
            throw new EntityNotFoundException("Тест", id);
        }

        _context.Tests.Remove(test);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TestDto>> GetTopRecentAsync(int count = 5)
    {
        await RefreshPublicationStatusesAsync();

        var tests = await _context.Tests
            .AsNoTracking()
            .OrderByDescending(t => t.PublishedAt)
            .ThenByDescending(t => t.Id)
            .Take(count)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TestDto>>(tests);
    }

    public async Task<IEnumerable<object>> GetTestCountByTypeAsync()
    {
        return await _context.Tests
            .AsNoTracking()
            .GroupBy(t => t.Type)
            .Select(g => new
            {
                Type = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetCourseStatsAsync()
    {
        return await _context.Courses
            .AsNoTracking()
            .Select(c => new
            {
                Course = c.Name,
                AvgDuration = c.Tests.Average(t => (double?)t.DurationMinutes) ?? 0,
                AvgPassingScore = c.Tests.Average(t => (double?)t.PassingScore) ?? 0
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetDirectionAveragesAsync()
    {
        return await _context.Directions
            .AsNoTracking()
            .Select(d => new
            {
                Direction = d.Name,
                AvgPassingScore = d.Tests.Average(t => (double?)t.PassingScore) ?? 0,
                AvgDuration = d.Tests.Average(t => (double?)t.DurationMinutes) ?? 0
            })
            .OrderByDescending(x => x.AvgPassingScore)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetTestTimelineByPublicAsync()
    {
        return await _context.Tests
            .AsNoTracking()
            .Where(t => t.PublishedAt != default)
            .GroupBy(t => new
            {
                t.IsPublic,
                Year = t.PublishedAt.Year,
                Month = t.PublishedAt.Month
            })
            .Select(g => new
            {
                g.Key.IsPublic,
                g.Key.Year,
                g.Key.Month,
                Count = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenByDescending(x => x.IsPublic)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetTopGroupsByTestCountAsync(int top = 10)
    {
        return await _context.Tests
            .AsNoTracking()
            .SelectMany(t => t.Groups.Select(g => new
            {
                Group = g.Name,
                TestId = t.Id
            }))
            .GroupBy(x => x.Group)
            .Select(g => new
            {
                Group = g.Key,
                TestCount = g.Select(x => x.TestId).Distinct().Count()
            })
            .OrderByDescending(x => x.TestCount)
            .Take(top)
            .ToListAsync();
    }

    #region Private Methods

    private async Task UpdateMembersTest(Test test, TestDto testDto)
    {
        var studentIds = testDto.Students?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();
        var groupIds = testDto.Groups?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();
        var courseIds = testDto.Courses?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();
        var directionIds = testDto.Directions?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();
        var projectIds = testDto.Projects?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();

        if (_context.Entry(test).State == EntityState.Detached)
            _context.Attach(test);

        // Студенты
        await _context.Entry(test).Collection(t => t.Students).LoadAsync();
        test.Students.Clear();
        if (studentIds.Length > 0)
        {
            var students = await _context.Students
                .Where(s => studentIds.Contains(s.Id))
                .ToListAsync();

            var foundStudentIds = students.Select(s => s.Id).ToList();
            var missingStudentIds = studentIds.Except(foundStudentIds).ToList();
            if (missingStudentIds.Any())
            {
                throw new EntityNotFoundException($"Студенты с ID: {string.Join(", ", missingStudentIds)} не найдены");
            }

            foreach (var s in students)
                test.Students.Add(s);
        }

        // Группы
        await _context.Entry(test).Collection(t => t.Groups).LoadAsync();
        test.Groups.Clear();
        if (groupIds.Length > 0)
        {
            var groups = await _context.Groups
                .Where(g => groupIds.Contains(g.Id))
                .ToListAsync();

            var foundGroupIds = groups.Select(g => g.Id).ToList();
            var missingGroupIds = groupIds.Except(foundGroupIds).ToList();
            if (missingGroupIds.Any())
            {
                throw new EntityNotFoundException($"Группы с ID: {string.Join(", ", missingGroupIds)} не найдены");
            }

            foreach (var g in groups)
                test.Groups.Add(g);
        }

        // Курсы
        await _context.Entry(test).Collection(t => t.Courses).LoadAsync();
        test.Courses.Clear();
        if (courseIds.Length > 0)
        {
            var courses = await _context.Courses
                .Where(c => courseIds.Contains(c.Id))
                .ToListAsync();

            var foundCourseIds = courses.Select(c => c.Id).ToList();
            var missingCourseIds = courseIds.Except(foundCourseIds).ToList();
            if (missingCourseIds.Any())
            {
                throw new EntityNotFoundException($"Курсы с ID: {string.Join(", ", missingCourseIds)} не найдены");
            }

            foreach (var c in courses)
                test.Courses.Add(c);
        }

        // Направления
        await _context.Entry(test).Collection(t => t.Directions).LoadAsync();
        test.Directions.Clear();
        if (directionIds.Length > 0)
        {
            var directions = await _context.Directions
                .Where(d => directionIds.Contains(d.Id))
                .ToListAsync();

            var foundDirectionIds = directions.Select(d => d.Id).ToList();
            var missingDirectionIds = directionIds.Except(foundDirectionIds).ToList();
            if (missingDirectionIds.Any())
            {
                throw new EntityNotFoundException($"Направления с ID: {string.Join(", ", missingDirectionIds)} не найдены");
            }

            foreach (var d in directions)
                test.Directions.Add(d);
        }

        // Проекты
        await _context.Entry(test).Collection(t => t.Projects).LoadAsync();
        test.Projects.Clear();
        if (projectIds.Length > 0)
        {
            var projects = await _context.Projects
                .Where(p => projectIds.Contains(p.Id))
                .ToListAsync();

            var foundProjectIds = projects.Select(p => p.Id).ToList();
            var missingProjectIds = projectIds.Except(foundProjectIds).ToList();
            if (missingProjectIds.Any())
            {
                throw new EntityNotFoundException($"Проекты с ID: {string.Join(", ", missingProjectIds)} не найдены");
            }

            foreach (var p in projects)
                test.Projects.Add(p);
        }
    }

    private async Task RefreshPublicationStatusesAsync()
    {
        var now = DateTime.UtcNow;

        // 1) Опубликовать всё, что должно стать публичным
        var publishCandidates = await _context.Tests
            .AsNoTracking()
            .Where(t => !t.IsPublic && (t.PublishedAt != default || t.Deadline != default))
            .Select(t => new { t.Id, t.PublishedAt, t.Deadline })
            .ToListAsync();

        var toPublishIds = publishCandidates
            .Where(x => x.PublishedAt != default
                        && x.PublishedAt <= now
                        && (x.Deadline == default || x.Deadline > now))
            .Select(x => x.Id)
            .ToList();

        if (toPublishIds.Count > 0)
            await _context.Tests
                .Where(t => toPublishIds.Contains(t.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsPublic, true));

        // 2) Снять с публикации, если не задана дата или дедлайн истёк
        var unpublishCandidates = await _context.Tests
            .AsNoTracking()
            .Where(t => t.IsPublic && (t.PublishedAt == default || t.Deadline != default))
            .Select(t => new { t.Id, t.PublishedAt, t.Deadline })
            .ToListAsync();

        var toUnpublishIds = unpublishCandidates
            .Where(x => x.PublishedAt == default
                        || (x.Deadline != default && x.Deadline <= now))
            .Select(x => x.Id)
            .ToList();

        if (toUnpublishIds.Count > 0)
            await _context.Tests
                .Where(t => toUnpublishIds.Contains(t.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsPublic, false));
    }

    #endregion
}
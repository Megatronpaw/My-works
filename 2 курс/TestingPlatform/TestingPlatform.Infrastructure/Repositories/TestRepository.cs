using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure;
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

    public async Task<IEnumerable<TestDto>> GetAllAsync()
    {
        var tests = await _context.Tests.ToListAsync();
        return _mapper.Map<IEnumerable<TestDto>>(tests);
    }

    public async Task<TestDto> GetByIdAsync(int id)
    {
        var test = await _context.Tests
            .AsNoTracking()
            .FirstOrDefaultAsync(test => test.Id == id);

        if (test == null)
        {
            throw new Exception("Тест не найден.");
        }

        return _mapper.Map<TestDto>(test);
    }

    public async Task<int> CreateAsync(TestDto testDto)
    {
        var test = _mapper.Map<Test>(testDto);
        await _context.Tests.AddAsync(test);
        await _context.SaveChangesAsync();
        return test.Id;
    }

    public async Task UpdateAsync(TestDto testDto)
    {
        var test = await _context.Tests.FirstOrDefaultAsync(test => test.Id == testDto.Id);

        if (test == null)
        {
            throw new Exception("Тест не найден.");
        }

        // Обновляем только основные свойства теста
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

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var test = await _context.Tests.FirstOrDefaultAsync(test => test.Id == id);

        if (test == null)
        {
            throw new Exception("Тест не найден.");
        }

        _context.Tests.Remove(test);
        await _context.SaveChangesAsync();
    }
}
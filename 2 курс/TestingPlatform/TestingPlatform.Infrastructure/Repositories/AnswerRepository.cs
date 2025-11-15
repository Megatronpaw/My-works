using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure;
using TestingPlatform.Infrastructure.Exceptions;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AnswerRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AnswerDto>> GetAllAsync()
    {
        var answers = await _context.Answers
            .AsNoTracking()
            .ToListAsync();
        return _mapper.Map<IEnumerable<AnswerDto>>(answers);
    }

    public async Task<AnswerDto> GetByIdAsync(int id)
    {
        var answer = await _context.Answers
            .AsNoTracking()
            .FirstOrDefaultAsync(answer => answer.Id == id);

        if (answer == null)
        {
            throw new EntityNotFoundException("Ответ", id);
        }

        return _mapper.Map<AnswerDto>(answer);
    }

    public async Task<int> CreateAsync(AnswerDto answerDto)
    {
        var answer = _mapper.Map<Answer>(answerDto);

        // Проверяем существование вопроса
        var questionExists = await _context.Questions
            .AnyAsync(q => q.Id == answerDto.QuestionId);
        if (!questionExists)
        {
            throw new EntityNotFoundException("Вопрос", answerDto.QuestionId);
        }

        await _context.Answers.AddAsync(answer);
        await _context.SaveChangesAsync();
        return answer.Id;
    }

    public async Task UpdateAsync(AnswerDto answerDto)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(answer => answer.Id == answerDto.Id);

        if (answer == null)
        {
            throw new EntityNotFoundException("Ответ", answerDto.Id);
        }

        // Проверяем существование вопроса
        var questionExists = await _context.Questions
            .AnyAsync(q => q.Id == answerDto.QuestionId);
        if (!questionExists)
        {
            throw new EntityNotFoundException("Вопрос", answerDto.QuestionId);
        }

        answer.Text = answerDto.Text;
        answer.IsCorrect = answerDto.IsCorrect;
        answer.QuestionId = answerDto.QuestionId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var answer = await _context.Answers.FirstOrDefaultAsync(answer => answer.Id == id);

        if (answer == null)
        {
            throw new EntityNotFoundException("Ответ", id);
        }

        _context.Answers.Remove(answer);
        await _context.SaveChangesAsync();
    }
}
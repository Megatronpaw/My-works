using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure;
using TestingPlatform.Infrastructure.Exceptions;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public StudentRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = await _context.Students
            .Include(s => s.User)
            .ToListAsync();
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<StudentDto> GetByIdAsync(int id)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .Include(s => s.Tests)
            .AsNoTracking()
            .FirstOrDefaultAsync(student => student.Id == id);

        if (student == null)
        {
            throw new EntityNotFoundException("Студент", id);
        }

        return _mapper.Map<StudentDto>(student);
    }

    public async Task<int> CreateAsync(StudentDto studentDto)
    {
        var student = _mapper.Map<Student>(studentDto);

        var studentId = await _context.AddAsync(student);
        await _context.SaveChangesAsync();

        return studentId.Entity.Id;
    }

    public async Task UpdateAsync(StudentDto studentDto)
    {
        var student = await _context.Students.FirstOrDefaultAsync(student => student.Id == studentDto.Id);

        if (student == null)
        {
            throw new EntityNotFoundException("Студент", studentDto.Id);
        }

        student.Phone = studentDto.Phone;
        student.VkProfileLink = studentDto.VkProfileLink;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            throw new EntityNotFoundException("Студент", id);
        }

        _context.Users.Remove(student.User);
        await _context.SaveChangesAsync();
    }
}
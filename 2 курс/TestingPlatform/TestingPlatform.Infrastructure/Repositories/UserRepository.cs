using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure;
using TestingPlatform.Infrastructure.Exceptions;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _context.Users.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
        {
            throw new EntityNotFoundException("Пользователь", id);
        }

        return _mapper.Map<UserDto>(user);
    }

    public async Task<int> CreateAsync(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.MiddleName = userDto.MiddleName;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        await _context.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task UpdateAsync(UserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userDto.Id);

        if (user == null)
        {
            throw new EntityNotFoundException("Пользователь", userDto.Id);
        }

        if (_context.Users.Any(u => u.Login == userDto.Login && u.Id != userDto.Id))
            throw new ArgumentException($"Пользователь с логином {userDto.Login} уже существует.");

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.MiddleName = userDto.MiddleName;
        user.Role = userDto.Role;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
        {
            throw new EntityNotFoundException("Пользователь", id);
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
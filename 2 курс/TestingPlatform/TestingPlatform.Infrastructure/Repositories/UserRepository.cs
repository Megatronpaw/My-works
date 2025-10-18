using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Models;

namespace TestingPlatform.Infrastructure.Repositories;

public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    public List<User> GetAllAsync()
    {
        var users = appDbContext.Users.AsNoTracking().ToList();
        return users;
    }

    public User GetByIdAsync(int id)
    {
        var user = appDbContext.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Id == id);

        if (user is null)
            throw new Exception("Пользователь не найден.");

        return user;
    }

    public int CreateAsync(User user)
    {
        appDbContext.Users.Add(user);
        appDbContext.SaveChanges();

        return user.Id;
    }

    public void UpdateAsync(User user)
    {
        appDbContext.SaveChanges();
    }

    public void DeleteAsync(int id)
    {
        var user = appDbContext.Users.FirstOrDefault(u => u.Id == id);
        if (user is null)
            throw new Exception("Пользователь не найден.");

        appDbContext.Users.Remove(user);
        appDbContext.SaveChanges();
    }
}


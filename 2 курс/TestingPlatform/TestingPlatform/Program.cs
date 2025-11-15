using Microsoft.EntityFrameworkCore;
using practice.Middlewares;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure;
using TestingPlatform.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация репозиториев
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

// Регистрация AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps("TestingPlatform.Infrastructure"));
builder.Services.AddAutoMapper(cfg => cfg.AddMaps("TestingPlatform"));

var app = builder.Build();

// Обработка миграций с таймаутом
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        // Пытаемся выполнить миграции с таймаутом
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await db.Database.MigrateAsync(cts.Token);
    }
    catch (OperationCanceledException)
    {
        // Если миграции не удались, создаем базу заново
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }
    catch (Exception ex)
    {
        // Логируем ошибку и создаем базу заново
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при выполнении миграций. База данных будет пересоздана.");

        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
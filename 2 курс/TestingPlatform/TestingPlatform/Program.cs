using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using practice.Services;
using Serilog;
using Serilog.Formatting.Json;
using System.Text;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure;
using TestingPlatform.Infrastructure.Repositories;
using TestingPlatform.Middlewares;
using TestingPlatform.Services;
using TestingPlatform.Settings;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);


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
builder.Services.AddScoped<IAttemptRepository, AttemptRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
   {
       options.RequireHttpsMetadata = true;
       options.SaveToken = true;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidIssuer = jwtSettings.Issuer,
           ValidateAudience = true,
           ValidAudience = jwtSettings.Audience,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(key),
           ValidateLifetime = true
       };
   });



Log.Logger = new LoggerConfiguration()
   .MinimumLevel.Information()
   .WriteTo.Console()
   .WriteTo.File(
       formatter: new JsonFormatter(),
       path: "logs/structured-.json")
   .WriteTo.SQLite("logs/logs.db")
   .CreateLogger();



builder.Host.UseSerilog();

// Регистрация AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps("TestingPlatform.Infrastructure"));
builder.Services.AddAutoMapper(cfg => cfg.AddMaps("TestingPlatform"));

var app = builder.Build();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Testing Platform API",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
   {
       {
           new OpenApiSecurityScheme
           {
               Reference = new OpenApiReference
               {
                   Type = ReferenceType.SecurityScheme,
                   Id = "Bearer"
               },
           },
           new List<string>()
       }
   });

});



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
app.UseAuthentication();
app.UseAuthorization();

app.Run();
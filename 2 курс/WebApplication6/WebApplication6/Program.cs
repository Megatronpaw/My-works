using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

//using Microsoft.Data.SqlClient;
using Npgsql;
using practice.Config;
using practice.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
// Регистрация сервисов
builder.Services.AddScoped<EfPurchaseService>();
builder.Services.AddScoped<DapperPurchaseService>();


// Регистрация опций
builder.Services.Configure<DataAccessOptions>(
    builder.Configuration.GetSection(DataAccessOptions.SectionName));

builder.Services.AddScoped<IPurchaseService>(sp =>
{
    var options = sp.GetRequiredService<IOptions<DataAccessOptions>>().Value;
    return options.Provider.ToLowerInvariant() switch
    {
        "efcore" => sp.GetRequiredService<EfPurchaseService>(),
        "dapper" => sp.GetRequiredService<DapperPurchaseService>(),
        _ => throw new InvalidOperationException($"Неизвестный провайдер данных: {options.Provider}")
    };
});
builder.Services.AddScoped<DapperProductService>();
builder.Services.AddScoped<EfProductService>();

// Условная регистрация IProductService (аналогично IPurchaseService)
builder.Services.AddScoped<IProductService>(sp =>
{
    var options = sp.GetRequiredService<IOptions<DataAccessOptions>>().Value;
    return options.Provider.ToLowerInvariant() switch
    {
        "efcore" => sp.GetRequiredService<EfProductService>(),
        "dapper" => sp.GetRequiredService<DapperProductService>(),
        _ => throw new InvalidOperationException($"Неизвестный провайдер данных: {options.Provider}")
    };
});


// Автоматическое сопоставление snake_case (из БД) с PascalCase (модель C#)
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрируем IDbConnection как scoped сервис
builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers(); // Добавляем поддержку контроллеров

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
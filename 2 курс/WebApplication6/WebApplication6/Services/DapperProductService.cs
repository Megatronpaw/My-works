using Dapper;
using System.Data;

namespace practice.Services;

public class DapperProductService : IProductService
{
    private readonly IDbConnection _connection;

    public DapperProductService(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        const string sql = "SELECT * FROM store.products";
        return await _connection.QueryAsync<Product>(sql);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM store.products WHERE id = @Id";
        return await _connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task<Product> CreateAsync(ProductCreateDto dto)
    {
        const string sql = @"
            INSERT INTO store.products (title, price, created_at)
            VALUES (@Title, @Price, @CreatedAt)
            RETURNING id, title, price, created_at;";

        var parameters = new
        {
            dto.Title,
            dto.Price,
            CreatedAt = DateTime.UtcNow
        };

        return await _connection.QuerySingleAsync<Product>(sql, parameters);
    }

    public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
    {
        const string sql = @"
            UPDATE store.products
            SET title = @Title, price = @Price
            WHERE id = @Id";

        var parameters = new
        {
            Id = id,
            dto.Title,
            dto.Price
        };

        var rows = await _connection.ExecuteAsync(sql, parameters);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM store.products WHERE id = @Id";
        var rows = await _connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
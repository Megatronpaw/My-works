using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IDbConnection _db;

    public ClientsController(IDbConnection db) => _db = db;

    [HttpGet]
    public async Task<IEnumerable<Client>> GetAll()
        => await _db.QueryAsync<Client>("SELECT * FROM store.clients");

    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await _db.QueryFirstOrDefaultAsync<Client>(
            "SELECT * FROM store.clients WHERE id = @Id", new { Id = id });
        return client is null ? NotFound() : client;
    }

    [HttpPost]
    public async Task<ActionResult<Client>> Create(Client client)
    {
        var sql = @"
            INSERT INTO store.clients (name, email, phone, birthday, address, created_at)
            VALUES (@Name, @Email, @Phone, @Birthday, @Address, @CreatedAt)
            RETURNING id;";

        var id = await _db.QuerySingleAsync<int>(sql, new
        {
            client.Name,
            client.Email,
            client.Phone,
            client.Birthday,
            client.Address,
            CreatedAt = DateTime.UtcNow
        });

        client.Id = id;
        return CreatedAtAction(nameof(GetById), new { id }, client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Client client)
    {
        var sql = @"
            UPDATE store.clients 
            SET name = @Name, 
                email = @Email, 
                phone = @Phone, 
                birthday = @Birthday, 
                address = @Address
            WHERE id = @Id";

        var affected = await _db.ExecuteAsync(sql, new
        {
            client.Name,
            client.Email,
            client.Phone,
            client.Birthday,
            client.Address,
            Id = id
        });

        return affected == 0 ? NotFound() : NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var sql = "UPDATE store.clients SET deleted_at = @Now WHERE id = @Id AND deleted_at IS NULL";
        var affected = await _db.ExecuteAsync(sql, new { Now = DateTime.UtcNow, Id = id });
        return affected == 0 ? NotFound() : NoContent();
    }
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Client>>> Search(
    [FromQuery] string? name = null,
    [FromQuery] DateTime? minBirthday = null,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        // Базовый SQL с фильтрацией и пагинацией
        const string sql = @"
        SELECT * FROM store.clients 
        WHERE deleted_at IS NULL
          AND (@Name IS NULL OR name ILIKE '%' || @Name || '%')
          AND (@MinBirthday IS NULL OR birthday >= @MinBirthday)
        ORDER BY created_at DESC
        LIMIT @PageSize OFFSET @Offset";

        var parameters = new DynamicParameters();
        parameters.Add("Name", name, DbType.String);
        parameters.Add("MinBirthday", minBirthday?.Date, DbType.Date);
        parameters.Add("PageSize", pageSize, DbType.Int32);
        parameters.Add("Offset", (page - 1) * pageSize, DbType.Int32);

        var clients = await _db.QueryAsync<Client>(sql, parameters);
        return Ok(clients);
    }
}

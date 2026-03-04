using System.Data;
using Dapper;

namespace practice.Services
{
    public class DapperPurchaseService : IPurchaseService
    {
        private readonly IDbConnection _connection;

        public DapperPurchaseService(IDbConnection connection) => _connection = connection;

        public async Task<IEnumerable<Purchase>> GetAllAsync()
        {
            return await _connection.QueryAsync<Purchase>("SELECT * FROM store.purchases");
        }

        public async Task<Purchase?> GetByIdAsync(int id)
        {
            return await _connection.QueryFirstOrDefaultAsync<Purchase>(
                "SELECT * FROM store.purchases WHERE id = @Id", new { Id = id });
        }

        public async Task<Purchase> CreateAsync(PurchaseCreateDto dto)
        {
            const string sql = @"
                INSERT INTO store.purchases (client_id, product_id, quantity)
                VALUES (@ClientId, @ProductId, @Quantity)
                RETURNING id, client_id, product_id, quantity, purchase_date;";

            return await _connection.QuerySingleAsync<Purchase>(sql, dto);
        }

        public async Task<bool> UpdateAsync(int id, PurchaseUpdateDto dto)
        {
            const string sql = @"
                UPDATE store.purchases 
                SET client_id = @ClientId, product_id = @ProductId, quantity = @Quantity
                WHERE id = @Id";

            var rows = await _connection.ExecuteAsync(sql, new { Id = id, dto.ClientId, dto.ProductId, dto.Quantity });
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rows = await _connection.ExecuteAsync(
                "DELETE FROM store.purchases WHERE id = @Id", new { Id = id });
            return rows > 0;
        }

        public async Task<IEnumerable<TopCustomerDto>> GetTopCustomersAsync(int limit = 5)
        {
            const string sql = @"
                SELECT 
                    c.name AS ClientName,
                    SUM(p.price * pu.quantity) AS TotalSpent
                FROM store.purchases pu
                JOIN store.clients c ON pu.client_id = c.id
                JOIN store.products p ON pu.product_id = p.id
                WHERE c.deleted_at IS NULL
                GROUP BY c.id, c.name
                ORDER BY TotalSpent DESC
                LIMIT @Limit";

            return await _connection.QueryAsync<TopCustomerDto>(sql, new { Limit = limit });
        }
    }
}


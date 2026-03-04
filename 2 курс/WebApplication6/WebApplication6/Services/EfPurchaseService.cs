using Microsoft.EntityFrameworkCore;

namespace practice.Services
{
    public class EfPurchaseService : IPurchaseService
    {
        private readonly AppDbContext _context;

        public EfPurchaseService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Purchase>> GetAllAsync()
            => await _context.Purchases.ToListAsync();

        public async Task<Purchase?> GetByIdAsync(int id)
            => await _context.Purchases.FindAsync(id);

        public async Task<Purchase> CreateAsync(PurchaseCreateDto dto)
        {
            var purchase = new Purchase
            {
                ClientId = dto.ClientId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return purchase;
        }

        public async Task<bool> UpdateAsync(int id, PurchaseUpdateDto dto)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null) return false;

            purchase.ClientId = dto.ClientId;
            purchase.ProductId = dto.ProductId;
            purchase.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null) return false;

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TopCustomerDto>> GetTopCustomersAsync(int limit = 5)
        {
            return await _context.Purchases
                .Where(p => p.Client!.DeletedAt == null)
                .GroupBy(p => new { p.Client!.Id, p.Client!.Name })
                .Select(g => new TopCustomerDto
                {
                    ClientName = g.Key.Name,
                    TotalSpent = g.Sum(p => p.Product!.Price * p.Quantity)
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(limit)
                .ToListAsync();
        }
    }
}


public interface IPurchaseService
{
    Task<IEnumerable<Purchase>> GetAllAsync();
    Task<Purchase?> GetByIdAsync(int id);
    Task<Purchase> CreateAsync(PurchaseCreateDto dto);
    Task<bool> UpdateAsync(int id, PurchaseUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<TopCustomerDto>> GetTopCustomersAsync(int limit = 5);
}


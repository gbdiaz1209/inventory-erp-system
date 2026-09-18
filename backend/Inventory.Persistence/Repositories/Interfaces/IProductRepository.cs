using Inventory.Persistence.Entities;

public interface IProductRepository
{
    Task<Product> CreateAsync(Product entity);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task UpdateAsync(Product entity);
    Task DeleteAsync(int id);
}
using Inventory.Persistence.Contexts;
using Inventory.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly InventoryDbContext _context;
    public CategoryRepository(InventoryDbContext context) => _context = context;

    public async Task<IEnumerable<Category>> GetAllAsync()
        => await _context.Categories.ToListAsync();

    public async Task<Category> CreateAsync(Category Category)
    {
        await _context.Categories.AddAsync(Category);
        await _context.SaveChangesAsync(); 
        return Category; 
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Category Category)
    {
        _context.Categories.Update(Category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await GetByIdAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

    }
}
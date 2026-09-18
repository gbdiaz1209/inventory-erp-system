using Inventory.Persistence.Contexts;
using Inventory.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context; //el repositorio tenga "las herramientas y el permiso" de hablar con la base de datos.

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(Product entity)
    {
        await _context.Products.AddAsync(entity);//BD tengo este producto nuevo
        await _context.SaveChangesAsync();//ve a la bodega y guarda físicamente todo lo que tengas pendiente".
        return entity;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task UpdateAsync(Product entity)
    {
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }
    // En tu archivo ProductRepository.cs
    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity != null)
        {
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

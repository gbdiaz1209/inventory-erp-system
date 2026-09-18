using Inventory.Persistence.Contexts;
using Inventory.Persistence.Entities;
using Inventory.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly InventoryDbContext _repository; //el repositorio tenga "las herramientas y el permiso" de hablar con la base de datos.

    public SupplierRepository(InventoryDbContext repository)
    {
        _repository = repository;
    }

    public async Task<Supplier> CreateAsync(Supplier entity)
    {
        await _repository.Supplier.AddAsync(entity);//BD tengo este Supplier nuevo
        await _repository.SaveChangesAsync();//ve a la bodega y guarda físicamente todo lo que tengas pendiente".
        return entity;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _repository.Supplier.ToListAsync();
    }

    public async Task<Supplier> GetByIdAsync(int id)
    {
        return await _repository.Supplier.FindAsync(id);
    }

    public async Task UpdateAsync(Supplier entity)
    {
        _repository.Supplier.Update(entity);
        await _repository.SaveChangesAsync();
    }
}
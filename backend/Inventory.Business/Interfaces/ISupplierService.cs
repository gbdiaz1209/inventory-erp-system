using Inventory.DTOs;

namespace Inventory.Business.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllSupplierAsync();
        Task<SupplierDto?> GetSupplierByIdAsync(int id);
        Task CreateSupplierAsync(SupplierDto supplierDto);
        Task UpdateSupplierAsync(int id, SupplierDto supplierDto);
     
    }
}
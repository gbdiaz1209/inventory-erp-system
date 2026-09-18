using Inventory.Business.Extensions; // Para usar .ToDto() y .ToEntity()
using Inventory.Business.Interfaces;
using Inventory.DTOs;
using Inventory.Persistence.Repositories.Interfaces;


namespace Inventory.Business.DomainServices
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public async Task<IEnumerable<SupplierDto>> GetAllSupplierAsync()
        {
            var supplier = await _supplierRepository.GetAllAsync();
            // Convertimos la lista de Entidades a DTOs
            return supplier.Select(s => s.ToDto());
        }
        public async Task<SupplierDto?> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            return supplier?.ToDto();
        }
        public async Task CreateSupplierAsync(SupplierDto supplierDto)
        {
            var entity = supplierDto.ToEntity();
            await _supplierRepository.CreateAsync(entity);
        }
        public async Task UpdateSupplierAsync(int id, SupplierDto supplierDto)
        {
            var existingSupplier = await _supplierRepository.GetByIdAsync(id);
            if (existingSupplier == null) throw new Exception("Proveedor no encontrado");
            {
                existingSupplier.Name = supplierDto.Name;
                existingSupplier.TaxId = supplierDto.TaxId;
                existingSupplier.Address= supplierDto.Address;
                existingSupplier.Email= supplierDto.Email;
                existingSupplier.ContactPerson=supplierDto.ContactPerson;
                existingSupplier.Phone= supplierDto.Phone;
                existingSupplier.IsActive = supplierDto.IsActive;

                await _supplierRepository.UpdateAsync(existingSupplier);
            }
        }

    }
}
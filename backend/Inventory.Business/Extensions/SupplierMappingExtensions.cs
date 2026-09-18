using Inventory.DTOs;
using Inventory.Persistence.Entities;
using System.Runtime.Versioning;


namespace Inventory.Business.Extensions
{
    public static class SupplierMappingExtensions
    {
        public static SupplierDto ToDto(this Supplier supplier)
        {
            return new SupplierDto { 
                Id = supplier.Id, 
                Name = supplier.Name,
                TaxId = supplier.TaxId,
                Address = supplier.Address,
                Phone=supplier.Phone, 
                Email=supplier.Email, 
                ContactPerson = supplier.ContactPerson,
                IsActive = supplier.IsActive,
             };
        }

        public static Supplier ToEntity(this SupplierDto dto)
        {
            return new Supplier { 
                Id = dto.Id, 
                Name = dto.Name,
                TaxId = dto.TaxId,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                ContactPerson = dto.ContactPerson,
                IsActive = dto.IsActive,
            };
        }
    }
}

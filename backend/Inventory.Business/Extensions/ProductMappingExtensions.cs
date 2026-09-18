using Inventory.DTOs;
using Inventory.Persistence.Entities;

namespace Inventory.Business.Extensions;

public static class ProductMappingExtensions
{
    // Convierte ProductDTO a Entidad
    public static Product ToEntity(this ProductDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Price = dto.Price,
        Stock = dto.Stock
    };

    // Convierte ProductoEntidad a DTO
    public static ProductDto ToDto(this Product entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Price = entity.Price,
        Stock = entity.Stock
    };
}
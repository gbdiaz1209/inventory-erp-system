using Inventory.DTOs;
using Inventory.Persistence.Entities;


namespace Inventory.Business.Extensions
{
    public static class CategoryMappingExtensions
    {
        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public static Category ToEntity(this CategoryDto dto)
        {
            return new Category { Id = dto.Id, Name = dto.Name };
        }
    }
}

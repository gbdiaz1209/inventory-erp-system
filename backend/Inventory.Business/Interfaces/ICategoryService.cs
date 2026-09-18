using Inventory.DTOs;

namespace Inventory.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(CategoryDto categoryDto);
        Task UpdateCategoryAsync(int id, CategoryDto categoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
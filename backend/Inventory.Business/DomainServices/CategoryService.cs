using Inventory.Business.Extensions; // Para usar .ToDto() y .ToEntity()
using Inventory.Business.Interfaces;
using Inventory.DTOs;
using Inventory.Persistence.Repositories;


namespace Inventory.Business.DomainServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            // Convertimos la lista de Entidades a DTOs
            return categories.Select(c => c.ToDto());
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category?.ToDto();
        }

        public async Task CreateCategoryAsync(CategoryDto categoryDto)
        {
            var entity = categoryDto.ToEntity();
            await _categoryRepository.AddAsync(entity);
        }

        public async Task UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory != null)
            {
                existingCategory.Name = categoryDto.Name;
                
                await _categoryRepository.UpdateAsync(existingCategory);
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
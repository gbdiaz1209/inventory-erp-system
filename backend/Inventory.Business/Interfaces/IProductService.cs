using Inventory.DTOs;

namespace Inventory.Business.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> CreateProductAsync(ProductDto productDto);

    Task UpdateProductAsync(int id, ProductDto productDto);

    Task DeleteProductAsync(int id); // Este es el nombre que llamaremos desde el controlador

}
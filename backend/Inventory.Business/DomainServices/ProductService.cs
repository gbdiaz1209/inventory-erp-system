using FluentValidation;               // Importamos validaciones
using Inventory.Business.Extensions;
using Inventory.Business.Interfaces;
using Inventory.DTOs;
using Inventory.Persistence.Repositories; // Importamos nuestras extensiones

namespace Inventory.Business.DomainServices;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository; //Necesito quien lo guarde
    private readonly IValidator<ProductDto> _validator; //Necesito quien valide
    
    //Constructor

    public ProductService(IProductRepository repository, IValidator<ProductDto> validator)
    {
        _repository = repository; 
        _validator = validator; 
    }
       
    public async Task<ProductDto> CreateProductAsync(ProductDto dto)
    {
        // 1. VALIDACIÓN
        var validationResult = await _validator.ValidateAsync(dto); //FluentValidation toma el objeto que vino de Angular y
                                                                    //lo compara contra las reglas que definiste en producto 
        if (!validationResult.IsValid)//isValid y Errors son prop
            throw new ValidationException(validationResult.Errors);// Si no es válido, lanza el paquete fuera de la oficina

        // 2. Convertimos la "caja" (DTO) en "caja de almacén" (Entity) No guardamos aún, solo preparamos el objeto.
        var entity = dto.ToEntity();

        // 3. PERSISTENCIA entregamos la entidad al repositorio para que la guarde en Docker.
        // Guardamos el resultado en 'createdEntity' porque Docker le asignará un ID nuevo.
        var createdEntity = await _repository.CreateAsync(entity);
     
        // 4. RESPUESTA (De vuelta al cliente)
        return createdEntity.ToDto();
    }
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        // Usamos la extensión .ToDto() que creamos antes
        return products.Select(p => p.ToDto());
    }

    public async Task UpdateProductAsync(int id, ProductDto productDto)
    {
        // 1. Buscamos si el producto existe Antes de intentar cambiar algo,
        // el servicio (Domain) le pide al repositorio que traiga el producto original desde Docker.
        var existingProduct = await _repository.GetByIdAsync(id);
        if (existingProduct == null) throw new Exception("Producto no encontrado");

        // 2. Si valida Actualizamos los valores de la entidad con los del DTO
        existingProduct.Name = productDto.Name;
        existingProduct.Price = productDto.Price;
        existingProduct.Stock = productDto.Stock;

        // 3. Mandamos a guardar los cambios
       // Una vez que el objeto existingProduct ha sido "actualizado" en la memoria del servidor,
       // se lo enviamos de vuelta al repositorio para que él se encargue de la persistencia final.
        await _repository.UpdateAsync(existingProduct);
    }
    // En tu archivo ProductService.cs
    public async Task DeleteProductAsync(int id)
    {
        // 1. Verificamos si existe
        var existingProduct = await _repository.GetByIdAsync(id);
        if (existingProduct == null)
            throw new Exception("Producto no encontrado");

        // 2. Llamamos al repositorio para borrar
        await _repository.DeleteAsync(id);
    }

}

/*En un entorno real, sí deberíamos usarlo. Antes de asignar los valores al existingProduct, 
  deberíamos validar el productDto igual que lo hicimos en el Create para asegurar que el usuario
  no puso un precio negativo o un nombre vacío al editar.*/
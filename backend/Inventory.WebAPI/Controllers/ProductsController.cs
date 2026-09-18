using FluentValidation;
using Inventory.Business.Interfaces;
using Inventory.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service; // Ahora usamos el Servicio para funcionar,
                                               // Net busca el servicio product en Program y se lo inyecta
    public ProductsController(IProductService service)
    {
        _service = service; 
                            
    }
    // usamos el constructor para inyectar IProductService.
    // Esto permite que el controlador no tenga que saber cómo se actualiza un producto,
    // solo sabe a quién pedirle que lo haga (desacoplamiento).
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
    {
        // El servicio ya nos devuelve DTOs limpios
        return Ok(await _service.GetAllProductsAsync());
    }

   [HttpPost] 
    public async Task<ActionResult<ProductDto>> Post(ProductDto productDto)
    {
        try
        {
            // El (Controller) le entrega la caja al Orquestador (Service) invoca al createProd
            var result = await _service.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            /* 1-> el método CreatedAtAction le dice al cliente (Angular): "¡Éxito! El objeto se procesó y existe físicamente en servidor"  nameof(GetById) (La Dirección): Le dice al cliente: "Si quieres volver a ver este producto que acabas de crear, 
            usa el método GetById". 2-> { id = result.Id }, Le dice: "Busca el ID que acaba de generar la base de datos".
            3->result: Le devuelve al cliente el objeto completo tal cual quedó guardado*/
        }
        catch (ValidationException ex)
        {
            // Si el inspector encontró fallos, se devuelve aquí
            return BadRequest(ex.Errors.Select(e => e.ErrorMessage));
        }
    }

    //1. El Verbo o Método HTTP - 2. La URL (Ruta o Endpoint)
    [HttpPut("{id}")] 
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
    //Frombody: Le indica al controlador que la información nueva del producto no viene en la URL, sino escondida en el "cuerpo"  de la petición HTTP que envía Angular.
    {
      //La validación inicial: El controlador recibe el ProductDto. Si el formato del JSON está mal, .NET lo rebota aquí mismo.
      //En una actualizacion se pide al repositorio que vaya al docker y traiga la entidad 
        await _service.UpdateProductAsync(id, productDto);
      return NoContent(); // Respuesta estándar 204: "Todo salió bien, no hay nada más que decir"
    }

    // En tu archivo ProductsController.cs

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteProductAsync(id);
            return NoContent(); // 204: Eliminación exitosa
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message); // Si no lo encuentra, devolvemos 404
        }
    }
}
using Inventory.Business.DTOs;
using Inventory.Business.Interfaces;
using Inventory.Persistence.Enum;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/inventorymovement")] // 👈 Forzamos la ruta estricta para evitar desajustes de enrutamiento
public class InventoryMovementController : ControllerBase
{
    private readonly IInventoryMovementService _movementService;
    // usamos el Servicio para funcionar,Net busca el servicio Movimiento de Inventario en Program y se lo inyecta
    // No dependemos de una clase concreta, sino de un contrato.

    // Inyección de Dependencias vía Constructor:
    public InventoryMovementController(IInventoryMovementService movementService)
    // .NET localiza la implementación registrada en Program.cs y la suministra automáticamente aquí.
    {
        _movementService = movementService;
        // El controlador queda "desacoplado": no le importa la lógica interna del servicio, 
        // solo confía en que el servicio cumplirá con los métodos definidos en la interfaz.
    }


    /// <summary>
    /// Registra un nuevo movimiento de inventario.
    /// </summary>
    /// <remarks>
    /// Este endpoint valida que el producto exista y que haya stock suficiente 
    /// en caso de ser una Salida.
    /// </remarks>
    /// <param name="request">DTO con ProductId, Quantity y Type (0=Entrada, 1=Salida).</param>
    /// <returns>Retorna un mensaje de éxito o un error detallado.</returns>
    /// <response code="200">El movimiento fue registrado y el stock actualizado.</response>
    /// <response code="400">Datos inválidos o stock insuficiente.</response>


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] InventoryMovementCreateDto request)
    {
        // El servicio ahora recibe el DTO directamente
        //Ejecuta la lógica
        var result = await _movementService.RegisterMovementAsync(request);
        if (!result)
        {
            // 2. Caso de Error: Si result es false, entra aquí y termina.
            return BadRequest(new { message = "No se pudo registrar el movimiento. Verifica stock o ID." });
        }

        return Ok(new { message = "Movimiento registrado con éxito." });
    }


    /// <summary>
    /// Obtiene el historial completo de movimientos de inventario.
    /// </summary>
    /// <returns>Lista de movimientos registrados.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Llamamos al servicio para que traiga la lista de movimientos
        // Asegúrate de que el método en tu interfaz se llame así o ajústalo a tu lógica
        var movements = await _movementService.GetAllMovementsAsync();
        return Ok(movements);
    }
}
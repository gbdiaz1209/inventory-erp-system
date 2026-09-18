using Inventory.Business.Interfaces;
using Inventory.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var suppliers = await _supplierService.GetAllSupplierAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null) return NotFound($"Proveedor con ID {id} no encontrado.");

            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SupplierDto supplierDto)
        {
            // El servicio ya se encarga de FluentValidation. 
            // Si falla, el middleware de FluentValidation o tu try-catch atraparán el error.
            await _supplierService.CreateSupplierAsync(supplierDto);
            return CreatedAtAction(nameof(GetById), new { id = supplierDto.Id }, supplierDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierDto supplierDto)
        {
            await _supplierService.UpdateSupplierAsync(id, supplierDto);
            return NoContent();
        }

    }
}
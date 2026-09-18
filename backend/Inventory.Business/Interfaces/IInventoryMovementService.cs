using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory.Business.DTOs;

namespace Inventory.Business.Interfaces
{
    public interface IInventoryMovementService
    {
        Task<bool> RegisterMovementAsync(InventoryMovementCreateDto request);

        //  Actualizado a InventoryMovementCreateDto para que coincida exactamente
        //  con el servicio
        Task<IEnumerable<InventoryMovementCreateDto>> GetAllMovementsAsync();
    }
}
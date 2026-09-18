using Inventory.Business.DTOs;
using Inventory.Business.Interfaces;
using Inventory.Persistence.Contexts;
using Inventory.Persistence.Entities;
using Inventory.Persistence.Enum;
using Microsoft.EntityFrameworkCore; // 🔌 Necesario para ToListAsync() y Select()
using System;
using System.Collections.Generic; // 🔌 Necesario para IEnumerable<T>
using System.Linq; // 🔌 Necesario para las consultas LINQ
using System.Threading.Tasks;

namespace Inventory.Business.Interfaces.Repositories
{
    public class InventoryMovementService : IInventoryMovementService
    {
        private readonly InventoryDbContext _context;

        public InventoryMovementService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterMovementAsync(InventoryMovementCreateDto dto)
        {
            // 1. Buscamos el producto usando el ID del DTO

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) return false;

            //y, solo si todo está bien, transforma esos datos en una entidad de base de datos (InventoryMovement) para guardarlos.
            // Convertimos el int del DTO al Enum de nuestra persistencia
            var movementType = (MovementType)dto.Type;

            // 2. Lógica de negocio (Validación de stock para salidas)
            if (movementType == MovementType.Exit && product.Stock < dto.Quantity)
            {
                return false;
            }

            // 3. Creamos la entidad de movimiento para el historial
            var movement = new InventoryMovement
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Type = movementType,
                Date = DateTime.Now
            };

            // 4. Actualizamos el stock del producto en memoria
            if (movementType == MovementType.Entry)
                product.Stock += dto.Quantity;
            else
                product.Stock -= dto.Quantity;

            // 5. Guardamos todo en la base de datos
            await _context.InventoryMovements.AddAsync(movement);
            //CHANGE TRACKING)
            /*Porque nunca escribimos una línea que diga _context.Products.Update(product) para actualizar el producto?*/

            /*El Rastreador: Cuando hiciste _context.Products.FindAsync(id), EF no solo te trajo los datos, sino que se quedó
             * "observando" ese objeto product. Al hacer un cambio en product.Stock += quantity;
             * tú modificaste el objeto en la memoria de C#. EF Core se dio cuenta de que ese objeto ya no es igual al que salió de 
             * la base de datos. 
             */
            await _context.SaveChangesAsync();

            /* Cuando ejecutas await _context.SaveChangesAsync();, Entity Framework hace una inspección rápida y dice:
             * "Tengo un Movimiento nuevo para insertar (Add)." "Y veo que el Producto que saqué antes cambió su Stock,
             * así que voy a generar un UPDATE automáticamente."*/

            return true;
        }

        // 📋 HISTORIAL DE MOVIMIENTOS: Implementación del método que faltaba 
        public async Task<IEnumerable<InventoryMovementCreateDto>> GetAllMovementsAsync()
        {
            return await _context.InventoryMovements
                .Select(m => new InventoryMovementCreateDto
                {
                    Id = m.Id,
                    ProductId = m.ProductId,
                    Quantity = m.Quantity,
                    Type = (int)m.Type,
                    Date = m.Date
                })
                .ToListAsync();
        }
    }
}
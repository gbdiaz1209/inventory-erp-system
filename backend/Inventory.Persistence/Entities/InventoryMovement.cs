using Inventory.Persistence.Enum; 



namespace Inventory.Persistence.Entities


{
    public class InventoryMovement
    {
        public int Id { get; set; }

        // La fecha se asigna automáticamente al momento de crear el objeto
        public DateTime Date { get; set; } = DateTime.Now;

        public int Quantity { get; set; }

        // Aquí usamos el Enum que creamos (Entry, Exit, Adjustment)
        public MovementType Type { get; set; }

        // --- Relación con la tabla Product ---

        // Esta es la Foreign Key (Llave foránea)
        public int ProductId { get; set; }

        // Propiedad de navegación: permite acceder a los datos del producto desde el movimiento
        public virtual Product Product { get; set; }
    }
}
namespace Inventory.Business.DTOs
{
    public class InventoryMovementCreateDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Type { get; set; }
        public DateTime Date { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Inventory.Persistence.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
       
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CategoryId { get; set; } //la llave

        // propiedad de navegación (Lo que le falta al DbContext)
        public virtual Category? Category { get; set; } //Product conoce a su Madre Categoria
        public virtual Supplier? Supplier { get; set; }
    }
       
}
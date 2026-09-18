using System.ComponentModel.DataAnnotations;

namespace Inventory.Persistence.Entities
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string TaxId { get; set; } // Ejemplo: NIT, RUC o RUT

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ContactPerson { get; set; }

        public bool IsActive { get; set; } = true;

        // Propiedad de navegación: Un proveedor puede suministrar muchos productos
        public virtual ICollection<Product> Products { get; set; }

    }
}
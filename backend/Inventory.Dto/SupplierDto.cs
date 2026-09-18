
using System.ComponentModel.DataAnnotations;

namespace Inventory.DTOs
{
    public class SupplierDto
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

    }
}
namespace Inventory.Persistence.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Propiedad de navegación: Una categoría tiene muchos productos
        //virtual e ICollection es para que EF pueda manejar la relación de "Uno a Muchos" de forma eficiente.
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        //La madre conoce a sus hijos
    }
}
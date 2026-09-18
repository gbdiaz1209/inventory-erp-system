using Inventory.Persistence.Entities;
using Microsoft.EntityFrameworkCore;


namespace Inventory.Persistence.Contexts
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<Supplier> Supplier { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<InventoryMovement> InventoryMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                 modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category) //Propiedades de navegacion de Product
                .HasForeignKey(p => p.CategoryId)
                .IsRequired(false); 


            // Configuración de precisión para precio del Producto
                modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }
    }
}

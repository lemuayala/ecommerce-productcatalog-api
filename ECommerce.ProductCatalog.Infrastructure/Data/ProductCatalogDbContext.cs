using ECommerce.ProductCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductCatalog.Infrastructure.Data;

public class ProductCatalogDbContext : DbContext
{
    // Constructor para la inyección de dependencias
    public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : base(options) { }

    // DbSet para cada entidad que EF Core debe gestionar
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuración para la entidad Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                .IsRequired()       
                .HasMaxLength(100); 
            entity.Property(c => c.Description)
                .HasMaxLength(500);
        });

        // Configuración para la entidad Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(p => p.Description)
                .HasMaxLength(1000);
            entity.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Configuración de la relación con Category
            // Un producto tiene una categoría (propiedad de navegación)
            entity.HasOne(p => p.Category)
                // Una categoría puede tener muchos productos (no definimos la colección inversa en Category)
                .WithMany()                     
                .HasForeignKey(p => p.CategoryId) 
                .IsRequired();   
        });
    }
}
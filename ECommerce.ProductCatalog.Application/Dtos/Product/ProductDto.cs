using ECommerce.ProductCatalog.Application.Dtos.Category;

namespace ECommerce.ProductCatalog.Application.Dtos.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto? Category { get; set; } 
    }
}
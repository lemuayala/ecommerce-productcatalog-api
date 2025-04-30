using System.ComponentModel.DataAnnotations;

namespace ECommerce.ProductCatalog.Api.Dtos.Category
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
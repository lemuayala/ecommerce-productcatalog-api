using System.ComponentModel.DataAnnotations;

namespace ECommerce.ProductCatalog.Api.Dtos.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
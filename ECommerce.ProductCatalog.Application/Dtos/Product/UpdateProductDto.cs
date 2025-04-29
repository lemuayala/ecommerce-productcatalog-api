using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.ProductCatalog.Application.Dtos.Product
{
    public class UpdateProductDto
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(200)]
        public string? Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public int? CategoryId { get; set; }
    }
}
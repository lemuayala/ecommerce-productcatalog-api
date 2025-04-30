using ECommerce.ProductCatalog.Domain.Entities;
using ECommerce.ProductCatalog.Domain.Interfaces.Repositories;

namespace ECommerce.ProductCatalog.Infrastructure.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ProductCatalogDbContext context) : base(context)
    {
    }
}
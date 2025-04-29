using ECommerce.ProductCatalog.Application.Interfaces;
using ECommerce.ProductCatalog.Domain.Entities;

namespace ECommerce.ProductCatalog.Infrastructure.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ProductCatalogDbContext context) : base(context)
    {
    }
}
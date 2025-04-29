using ECommerce.ProductCatalog.Domain.Entities;

namespace ECommerce.ProductCatalog.Application.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
}
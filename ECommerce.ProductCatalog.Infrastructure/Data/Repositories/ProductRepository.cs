using ECommerce.ProductCatalog.Domain.Entities;
using ECommerce.ProductCatalog.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductCatalog.Infrastructure.Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ProductCatalogDbContext context) : base(context)
    {
    }

    // Implementación del método específico de IProductRepository
    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .AsNoTracking()           // Para consulta de solo lectura
            .ToListAsync(cancellationToken);
    }
    
    public override async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
using ECommerce.ProductCatalog.Application.Interfaces;
using ECommerce.ProductCatalog.Infrastructure.Data.Repositories;

namespace ECommerce.ProductCatalog.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductCatalogDbContext _context;
    public ICategoryRepository Categories { get; private set; }
    public IProductRepository Products { get; private set; }

    public UnitOfWork(ProductCatalogDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        // Creamos las instancias de los repositorios pasándoles el MISMO DbContext
        Categories = new CategoryRepository(_context);
        Products = new ProductRepository(_context);
    }
    
    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    // Implementación de IAsyncDisposable para liberar el DbContext
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
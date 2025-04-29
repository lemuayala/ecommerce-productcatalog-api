using ECommerce.ProductCatalog.Application.Interfaces.Repositories;

namespace ECommerce.ProductCatalog.Application.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }

    // Método para confirmar todos los cambios realizados en esta "unidad"
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
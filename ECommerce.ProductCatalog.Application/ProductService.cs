using ECommerce.ProductCatalog.Application.Interfaces;
using ECommerce.ProductCatalog.Application.Interfaces.Repositories; 
using ECommerce.ProductCatalog.Application.Interfaces.Services; 
using ECommerce.ProductCatalog.Domain.Entities;

namespace ECommerce.ProductCatalog.Application;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository; 

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _productRepository = _unitOfWork.Products;
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _productRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _productRepository.GetAllAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productRepository.GetByIdAsync(product.Id, cancellationToken);
        if (existingProduct == null)
        {
            return false; 
        }

        _productRepository.Update(product);
        await _unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var productToDelete = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (productToDelete != null)
        {
            _productRepository.Delete(productToDelete);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _productRepository.FindAsync(p => p.CategoryId == categoryId, cancellationToken);
    }
}
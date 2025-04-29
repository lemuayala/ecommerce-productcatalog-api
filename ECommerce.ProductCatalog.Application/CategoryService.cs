using ECommerce.ProductCatalog.Application.Interfaces;
using ECommerce.ProductCatalog.Application.Interfaces.Repositories;
using ECommerce.ProductCatalog.Application.Interfaces.Services;
using ECommerce.ProductCatalog.Domain.Entities;

namespace ECommerce.ProductCatalog.Application;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = _unitOfWork.Categories;
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _categoryRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _categoryRepository.GetAllAsync(cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _categoryRepository.AddAsync(category, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _categoryRepository.Update(category);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var categoryToDelete = await _categoryRepository.GetByIdAsync(id, cancellationToken);
        if (categoryToDelete != null)
        {
            _categoryRepository.Delete(categoryToDelete);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
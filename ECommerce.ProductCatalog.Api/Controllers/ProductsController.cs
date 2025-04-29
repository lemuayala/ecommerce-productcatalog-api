using ECommerce.ProductCatalog.Application.Interfaces.Services;
using ECommerce.ProductCatalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ECommerce.ProductCatalog.Application.Dtos.Category;
using ECommerce.ProductCatalog.Application.Dtos.Product;

namespace ECommerce.ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return Ok(products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            Category = product.Category == null ? null : new CategoryDto
            {
                Id = product.Category.Id,
                Name = product.Category.Name,
                Description = product.Category.Description
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            CategoryId = createProductDto.CategoryId
        };
        await _productService.AddAsync(product, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        if (id != updateProductDto.Id)
        {
            return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingProduct = await _productService.GetByIdAsync(id, cancellationToken);
        if (existingProduct == null)
        {
            return NotFound();
        }
        existingProduct.Name = updateProductDto.Name ?? existingProduct.Name;
        existingProduct.Description = updateProductDto.Description ?? existingProduct.Description;
        existingProduct.Price = updateProductDto.Price ?? existingProduct.Price;
        existingProduct.CategoryId = updateProductDto.CategoryId ?? existingProduct.CategoryId;
        await _productService.UpdateAsync(existingProduct, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var existingProduct = await _productService.GetByIdAsync(id, cancellationToken);
        if (existingProduct == null)
        {
            return NotFound();
        }
        await _productService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId, CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductsByCategoryAsync(categoryId, cancellationToken);
        return Ok(products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId
        }));
    }
}
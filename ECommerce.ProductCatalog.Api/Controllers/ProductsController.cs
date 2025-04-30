using AutoMapper;
using ECommerce.ProductCatalog.Api.Dtos.Product;
using ECommerce.ProductCatalog.Application.Interfaces.Services;
using ECommerce.ProductCatalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMapper _mapper;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger, IMapper mapper)
    {
        _productService = productService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<ProductDto>(product));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var product = _mapper.Map<Product>(createProductDto);
        await _productService.AddAsync(product, cancellationToken);
        var productDto = _mapper.Map<ProductDto>(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, productDto);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        if (id != updateProductDto.Id) return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existingProduct = await _productService.GetByIdAsync(id, cancellationToken);
        if (existingProduct == null) return NotFound();

        _mapper.Map(updateProductDto, existingProduct);
        var updateResult = await _productService.UpdateAsync(existingProduct, cancellationToken);

        if (!updateResult) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
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
        return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
    }
}
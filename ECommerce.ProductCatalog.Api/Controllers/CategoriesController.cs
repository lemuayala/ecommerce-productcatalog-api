using ECommerce.ProductCatalog.Api.Dtos.Category;
using ECommerce.ProductCatalog.Application.Interfaces.Services;
using ECommerce.ProductCatalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllAsync(cancellationToken);
        return Ok(categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name, Description = c.Description }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetByIdAsync(id, cancellationToken);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(new CategoryDto { Id = category.Id, Name = category.Name, Description = category.Description });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var category = new Category { Name = createCategoryDto.Name, Description = createCategoryDto.Description };
        await _categoryService.AddAsync(category, cancellationToken);
        var categoryDto = new CategoryDto { Id = category.Id, Name = category.Name, Description = category.Description };
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken)
    {
        if (id != updateCategoryDto.Id)
        {
            return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existingCategory = await _categoryService.GetByIdAsync(id, cancellationToken);
        if (existingCategory == null)
        {
            return NotFound();
        }
        existingCategory.Name = updateCategoryDto.Name ?? existingCategory.Name;
        existingCategory.Description = updateCategoryDto.Description ?? existingCategory.Description;
        await _categoryService.UpdateAsync(existingCategory, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryService.GetByIdAsync(id, cancellationToken);
        if (existingCategory == null)
        {
            return NotFound();
        }
        await _categoryService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
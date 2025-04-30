using ECommerce.ProductCatalog.Api.Dtos.Product;
using FluentValidation;

namespace ECommerce.ProductCatalog.Api.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El nombre del producto es requerido.")
                .MaximumLength(200).WithMessage("El nombre del producto no puede exceder los 200 caracteres.");

            RuleFor(p => p.Description)
                .MaximumLength(1000).WithMessage("La descripción del producto no puede exceder los 1000 caracteres.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor que cero.");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("El CategoryId debe ser mayor que cero.");
        }
    }
}
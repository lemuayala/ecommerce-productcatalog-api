using ECommerce.ProductCatalog.Api.Dtos.Product;
using FluentValidation;

namespace ECommerce.ProductCatalog.Api.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("El Id debe ser mayor que cero.");

            RuleFor(p => p.Name)
                .MaximumLength(200).WithMessage("El nombre del producto no puede exceder los 200 caracteres.")
                .When(p => p.Name != null);

            RuleFor(p => p.Description)
                .MaximumLength(1000).WithMessage("La descripción del producto no puede exceder los 1000 caracteres.")
                .When(p => p.Description != null);

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor que cero.")
                .When(p => p.Price.HasValue);

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("El CategoryId debe ser mayor que cero.")
                .When(p => p.CategoryId.HasValue);
        }
    }
}
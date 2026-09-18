using FluentValidation;
using Inventory.DTOs;

public class ProductValidator : AbstractValidator<ProductDto> 
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            //Primero revisa si está vacío
            .NotEmpty().WithMessage("El nombre es obligatorio.")
           // Solo si pasa la primera, revisa el largo mínimo
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a cero.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
    }
}
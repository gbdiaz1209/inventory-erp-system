using FluentValidation;
using Inventory.DTOs;

public class SupplierValidator : AbstractValidator<SupplierDto> 
{
    public SupplierValidator()
    {
        RuleFor(s => s.Name)
                   .NotEmpty().WithMessage("El nombre es obligatorio.")
                   .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(s => s.TaxId)
            .NotEmpty().WithMessage("El identificador tributario (TaxId) es obligatorio.")
            .Length(5, 20).WithMessage("El TaxId debe tener entre 5 y 20 caracteres.");

        RuleFor(s => s.Email)
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .When(s => !string.IsNullOrEmpty(s.Email));
    }
}

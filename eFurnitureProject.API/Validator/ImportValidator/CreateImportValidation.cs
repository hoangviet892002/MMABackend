using eFurnitureProject.Application.ViewModels.ImportViewModels;
using eFurnitureProject.Domain.Entities;
using FluentValidation;

namespace eFurnitureProject.API.Validator.InventoryValidator
{
    public class CreateImportValidation : AbstractValidator<CreateImportDTO>
    {
        public CreateImportValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255)
               .WithMessage("Length of name must not exceed 255 characters");
            RuleFor(x => x.ImportDetail).NotEmpty();
        }
    }
}

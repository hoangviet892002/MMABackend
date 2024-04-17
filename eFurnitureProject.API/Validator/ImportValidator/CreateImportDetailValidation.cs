using eFurnitureProject.Application.ViewModels.ImportDetailViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.ImportValidator
{
    public class CreateImportDetailValidation : AbstractValidator<CreateImportDetailDTO>
    {
        public CreateImportDetailValidation()
        {
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0)
                .WithMessage("Price must greater than 0");
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0)
                .WithMessage("Quantity must greater than 0");
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}

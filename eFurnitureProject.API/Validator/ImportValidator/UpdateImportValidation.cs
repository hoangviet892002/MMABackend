using eFurnitureProject.Application.ViewModels.ImportViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.ImportValidator
{
    public class UpdateImportValidation : AbstractValidator<UpdateImportDTO>
    {
        public UpdateImportValidation()
        {
            RuleFor(x => x.ImportId).NotEmpty();
            RuleFor(x => x.Status).NotEmpty().GreaterThanOrEqualTo(1).LessThanOrEqualTo(3)
                .WithMessage("Status value must between 1 and 3");
        }
    }
}

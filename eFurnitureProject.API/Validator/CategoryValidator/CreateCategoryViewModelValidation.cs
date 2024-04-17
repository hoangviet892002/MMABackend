using eFurnitureProject.Application.ViewModels.CategoryViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.CategoryValidator
{
    public class CreateCategoryViewModelValidation : AbstractValidator<CreateCategoryViewModel>
    {
        public CreateCategoryViewModelValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100)
              .WithMessage("Name of category length must not exceed 100 characters");
        }
    }
}

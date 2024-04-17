using eFurnitureProject.Application.ViewModels.ProductDTO;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace eFurnitureProject.API.Validator.ProductValidator
{
    public class CreateProductValidation:AbstractValidator<CreateProductDTO>
    {
        public CreateProductValidation()
        {
            
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required");

                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required");

                RuleFor(x => x.Image)
                  
                    .NotEmpty().WithMessage("Status is required");

                RuleFor(x => x.CategoryId)
                    .NotEmpty().WithMessage("CategoryId is required");
            
        }
    }
}

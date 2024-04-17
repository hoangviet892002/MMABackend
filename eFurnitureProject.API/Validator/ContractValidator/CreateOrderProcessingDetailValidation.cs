using eFurnitureProject.Application.ViewModels.OrderProcessingDetailViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.ContractValidator
{
    public class CreateOrderProcessingDetailValidation : AbstractValidator<CreateOrderProcessingDetailDTO>
    {
        public CreateOrderProcessingDetailValidation()
        {
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0)
              .WithMessage("Quantity product must greater than 0");
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}

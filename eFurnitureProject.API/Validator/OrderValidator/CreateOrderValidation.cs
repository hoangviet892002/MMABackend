using eFurnitureProject.Application.ViewModels.OrderViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.OrderValidator
{
    public class CreateOrderValidation : AbstractValidator<CreateOrderDTO>
    {
        public CreateOrderValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress()
                .WithMessage("Email is invalid format!");
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^0[0-9]{9}$");
        }
    }
}

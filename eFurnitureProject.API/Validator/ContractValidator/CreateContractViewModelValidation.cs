using eFurnitureProject.Application.ViewModels.ContractViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.ContractValidator
{
    public class CreateContractViewModelValidation : AbstractValidator<CreateContractDTO>
    {
        public CreateContractViewModelValidation()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).MaximumLength(255)
                .WithMessage("Description contract length must not exceed 255 characters");
            RuleFor(x => x.Value).NotEmpty().GreaterThan(0)
                .WithMessage("Value contract must greater than 0");
            RuleFor(x => x.CustomerId);
            RuleFor(x => x.Pay).NotEmpty().GreaterThan(0).LessThanOrEqualTo(x => x.Value)
               .WithMessage("Pay contract must greater than 0 and less or equal to value of contract");
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress()
                .WithMessage("Email is invalid format!");
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^0[0-9]{9}$")
                .WithMessage("The phone number must have 10 digits and start with 0!");
            RuleFor(x => x.Address).NotEmpty();
            RuleForEach(x => x.Items).NotEmpty().SetValidator(new CreateOrderProcessingDetailValidation());
        }
    }
}

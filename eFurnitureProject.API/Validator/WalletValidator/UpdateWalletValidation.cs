using eFurnitureProject.Application.ViewModels.WalletViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.WalletValidator
{
    public class UpdateWalletValidation : AbstractValidator<UpdateWalletDTO>
    {
        public UpdateWalletValidation() 
        {
            RuleFor(x => x.Wallet)
                .GreaterThan(0).WithMessage("The amount must be greater than 0");
        }
    }
}

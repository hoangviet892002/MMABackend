using eFurnitureProject.Application.ViewModels.UserViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.UserValidator
{
    public class ChangePasswordValidation : AbstractValidator<UserPasswordDTO>
    {
        public ChangePasswordValidation()
        {
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long!")
            .Matches(@"[0-9]+")
                .WithMessage("Your password must contain at least one number!");
            RuleFor(x => x.OldPassword).Equal(x => x.ConfirmPassword)
                .WithMessage("Your password confirmed is wrong!");
        }
    }
}

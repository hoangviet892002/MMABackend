using eFurnitureProject.Application.ViewModels.UserViewModels;
using FluentValidation;

namespace eFurnitureProject.API.Validator.UserValidator
{
    public class UpdateUserValidation : AbstractValidator<UserUpdateDTO>
    {
        public UpdateUserValidation()
        {
            RuleFor(x => x.Name);
            RuleFor(x => x.Email).EmailAddress()
                .WithMessage("Email is invalid format!");
            RuleFor(x => x.PhoneNumber).Matches(@"^0[0-9]{9}$")
                .WithMessage("The phone number must have 10 digits and start with 0!");
            
        }
    }
}

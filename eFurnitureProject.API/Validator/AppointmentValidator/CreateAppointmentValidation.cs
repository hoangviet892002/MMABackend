using eFurnitureProject.Application.ViewModels.AppointmentViewModel;
using eFurnitureProject.Application.ViewModels.UserViewModels;
using FluentValidation;
namespace eFurnitureProject.API.Validator.AppointmentValidator
{
    public class CreateAppointmentValidation: AbstractValidator<CreateAppointmentDTO>
    {
        public CreateAppointmentValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Date)
                .NotEmpty()
                .Must(BeAValidDate).WithMessage("Date is invalid");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().Matches(@"^0[0-9]{9}$")
                .WithMessage("The phone number must have 10 digits and start with 0!");

            RuleFor(x => x.Email)
                .NotEmpty().NotEmpty().EmailAddress()
                .WithMessage("Email is invalid format!");


            RuleFor(x => x.Time)
    .NotEmpty().WithMessage("Time is required.")
    .Matches(@"^((?:[01]\d|2[0-3]):[0-5]\d|24:00)$")
        .WithMessage("Time must be in valid format, e.g., 00:00 to 24:00")
    .When(x => x.Time != null);
      

        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}

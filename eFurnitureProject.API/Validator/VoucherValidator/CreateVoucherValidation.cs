using eFurnitureProject.Application.ViewModels.VoucherDTO;
using FluentValidation;

namespace eFurnitureProject.API.Validator.VoucherValidator
{
    public class CreateVoucherValidation : AbstractValidator<CreateVoucherDTO>
    {
        public CreateVoucherValidation()
        {

            RuleFor(x => x.VoucherName)
                .NotEmpty().WithMessage("VoucherName is required")
                .MaximumLength(25).WithMessage("VoucherName cannot exceed 25 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required")
                .LessThan(x => x.EndDate).WithMessage("StartDate must be before EndDate");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required")
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate");

            RuleFor(x => x.Percent)
                .GreaterThan(0).WithMessage("MinimumOrderValue must be greater than 0");       
            RuleFor(x => x.MinimumOrderValue)
                .GreaterThan(0).WithMessage("MinimumOrderValue must be greater than 0");

            RuleFor(x => x.MaximumDiscountAmount)
                .GreaterThan(0).WithMessage("MaximumDiscountAmount must be greater than 0");


        }

    }
}

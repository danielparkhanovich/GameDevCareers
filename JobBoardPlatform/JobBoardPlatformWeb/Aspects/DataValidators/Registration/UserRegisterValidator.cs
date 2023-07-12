using FluentValidation;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Registration
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterViewModel>
    {
        public UserRegisterValidator()
        {
            RuleFor(register => register.Email).EmailAddress();
            RuleFor(register => register.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                    .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).")
                    .Equal(customer => customer.RepeatPassword).WithMessage("Your password and repeat password do not match.");
            RuleFor(register => register.RepeatPassword)
                    .Equal(customer => customer.Password).WithMessage("Your password and repeat password do not match.");
            RuleFor(register => register.IsAcceptedTermsOfService).Equal(true).WithMessage("Please read and accept the terms of service before continuing.");
        }
    }
}

using FluentValidation;
using JobBoardPlatform.PL.Aspects.DataValidators.Common;
using JobBoardPlatform.PL.Aspects.DataValidators.Offers;
using JobBoardPlatform.PL.Aspects.DataValidators.Profile;
using JobBoardPlatform.PL.ViewModels.Models.Registration;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Registration
{
    public class CompanyPublishOfferAndRegisterValidator : AbstractValidator<CompanyPublishOfferAndRegisterViewModel>
    {
        public CompanyPublishOfferAndRegisterValidator()
        {
            // AddRulesForCompanyRegistrationData();
            // RuleFor(register => register.EditOffer.OfferDetails).SetValidator(new OfferFormDataValidator());
        }

        private void AddRulesForCompanyRegistrationData()
        {
            RuleFor(register => register.CompanyRegistrationData.ProfileImage).SetValidator(new ProfileImageValidator());
            RuleFor(register => register.CompanyRegistrationData.CompanyName).NotEmpty()
                .WithMessage("Please enter company name");
            RuleFor(register => register.CompanyRegistrationData.OfficeCity).NotEmpty()
                .WithMessage("Please company office city");
            RuleFor(register => register.CompanyRegistrationData.OfficeStreet).NotEmpty()
                .WithMessage("Please company office street");
            When(register => !string.IsNullOrEmpty(register.CompanyRegistrationData.CompanyWebsiteUrl), () =>
            {
                RuleFor(register => register.CompanyRegistrationData.CompanyWebsiteUrl!).SetValidator(new UrlValidator());
            });
        }
    }
}

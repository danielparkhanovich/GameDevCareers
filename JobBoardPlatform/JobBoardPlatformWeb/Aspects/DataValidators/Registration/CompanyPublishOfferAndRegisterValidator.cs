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
            AddRulesForCompanyRegistrationData();
            RuleFor(register => register.EditOffer.OfferDetails).SetValidator(new OfferFormDataValidator());
        }

        private void AddRulesForCompanyRegistrationData()
        {
            RuleFor(register => register.CompanyProfileData.ProfileImage).SetValidator(new ProfileImageValidator());
            RuleFor(register => register.CompanyProfileData.CompanyName).NotEmpty()
                .WithMessage("Please enter company name");
            RuleFor(register => register.CompanyProfileData.OfficeCity).NotEmpty()
                .WithMessage("Please enter company office city");
            RuleFor(register => register.CompanyProfileData.OfficeStreet).NotEmpty()
                .WithMessage("Please enter company office street");
            When(register => !string.IsNullOrEmpty(register.CompanyProfileData.CompanyWebsiteUrl), () =>
            {
                RuleFor(register => register.CompanyProfileData.CompanyWebsiteUrl!).SetValidator(new UrlValidator());
            });
        }
    }
}

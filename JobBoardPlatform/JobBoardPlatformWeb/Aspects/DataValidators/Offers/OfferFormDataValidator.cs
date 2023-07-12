using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.Aspects.DataValidators.Common;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Offers
{
    public class OfferFormDataValidator : AbstractValidator<INewOfferData>
    {
        public OfferFormDataValidator()
        {
            RuleFor(offerData => offerData.JobTitle).NotNull().WithMessage("Job title cannot be empty");
            RuleFor(offerData => offerData.Country).NotNull().WithMessage("Country cannot be empty");
            RuleFor(offerData => offerData.City).NotNull().WithMessage("City cannot be empty");
            RuleFor(offerData => offerData.WorkLocationType).NotEqual(0).WithMessage("Select work location type");
            RuleFor(offerData => offerData.EmploymentTypes).NotEmpty().WithMessage("Select employment type");
            AddEmploymentDetailsValidationRules();
            RuleFor(offerData => offerData.MainTechnologyType).NotEqual(0).WithMessage("Select main technology type");
            RuleFor(offerData => offerData.JobDescription).NotEmpty().WithMessage("Enter job description");
            AddApplicationsContactTypeValidationRules();
            AddAgreementsValidationRules();
        }

        private void AddEmploymentDetailsValidationRules()
        {
            When(offerData => offerData.EmploymentTypes.Length > 0, () => {
                RuleForEach(offerData => offerData.EmploymentTypes).NotEqual(0).WithMessage("Select employment type");
            });
            AddSalaryValidationRules();
        }

        private void AddSalaryValidationRules()
        {
            When(offerData => offerData.SalaryFromRange != null && offerData.SalaryToRange != null, () => {
                RuleForEach(offerData => offerData.SalaryFromRange).NotEqual(0).WithMessage("Enter from range");
                RuleForEach(offerData => offerData.SalaryToRange).NotEqual(0).WithMessage("Enter to range");
                When(offerData => offerData.SalaryFromRange!.Length == offerData.SalaryToRange!.Length, () =>
                {
                    RuleFor(offerData => offerData.SalaryFromRange).Must((offerData, salaryFromRange) =>
                    {
                        for (int i = 0; i < salaryFromRange!.Length; i++)
                        {
                            if (salaryFromRange[i] > offerData.SalaryToRange![i])
                            {
                                return false;
                            }
                        }
                        return true;
                    }).WithMessage("'Salary to' should be greater than 'salary from'");
                    RuleForEach(offerData => offerData.SalaryCurrencyType).NotEqual(0).WithMessage("Select currency type");
                });
            });
        }

        private void AddApplicationsContactTypeValidationRules()
        {
            RuleFor(offerData => offerData.ApplicationsContactType).NotEqual(0)
                .WithMessage("Select applications contact type");
            When(offerData => offerData.ApplicationsContactType == 1, () =>
            {
                RuleFor(offerData => offerData.ApplicationsContactEmail).EmailAddress();
            });
            When(offerData => offerData.ApplicationsContactType == 2, () =>
            {
                RuleFor(offerData => offerData.ApplicationsContactExternalFormUrl!).SetValidator(new UrlValidator());
            });
        }

        private void AddAgreementsValidationRules()
        {
            RuleFor(offerData => offerData.InformationClause).NotEmpty();
            When(offerData => offerData.IsDisplayConsentForFutureRecruitment, () =>
            {
                RuleFor(offerData => offerData.ConsentForFutureRecruitmentContent).NotEmpty()
                    .WithMessage("Enter consent for future recruitment or uncheck display consent for future recruitment checkbox"); ;
            });
            When(offerData => offerData.IsDisplayCustomConsent, () =>
            {
                RuleFor(offerData => offerData.CustomConsentContent).NotEmpty()
                    .WithMessage("Enter custom consent or uncheck display custom consent checkbox");
            });
        }
    }
}

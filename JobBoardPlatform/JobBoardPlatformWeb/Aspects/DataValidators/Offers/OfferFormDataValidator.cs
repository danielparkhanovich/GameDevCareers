using FluentValidation;
using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.PL.Aspects.DataValidators.Common;
using System.Text.RegularExpressions;

namespace JobBoardPlatform.PL.Aspects.DataValidators.Offers
{
    public class OfferFormDataValidator : AbstractValidator<IOfferData>
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
            RuleFor(offerData => offerData.JobDescription).NotNull()
                .Must((offerData, jobDescription) => GetInnerTextFromHtml(jobDescription) != string.Empty)
                .WithMessage("Enter job description");
            AddApplicationsContactTypeValidationRules();
            AddAgreementsValidationRules();
        }

        private void AddEmploymentDetailsValidationRules()
        {
            When(offerData => offerData.EmploymentTypes.Length > 0, () => {
                AddEmploymentTypeValidationRules();
                AddSalaryValidationRules();
            });
        }

        private void AddEmploymentTypeValidationRules()
        {
            RuleFor(offerData => offerData.EmploymentTypes).Must(employmentTypes =>
            {
                foreach (var employment in employmentTypes)
                {
                    foreach (var another in employmentTypes)
                    {
                        if (employment == another)
                        {
                            continue;
                        }
                        else if (employment.TypeId == another.TypeId)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }).WithMessage("Employment types cannot be same");

            RuleForEach(offerData => offerData.EmploymentTypes).Must(employment =>
            {
                return employment.TypeId != 0;
            }).WithMessage("Select employment type");
        }

        private void AddSalaryValidationRules()
        {
            RuleForEach(offerData => offerData.EmploymentTypes).Must(employment =>
            {
                if (!employment.SalaryFromRange.HasValue || !employment.SalaryToRange.HasValue)
                {
                    return true;
                }
                return employment.SalaryFromRange < employment.SalaryToRange;
            }).WithMessage("'Salary to' should be greater than 'salary from'");

            RuleForEach(offerData => offerData.EmploymentTypes).Must(employment =>
            {
                if (!employment.SalaryToRange.HasValue)
                {
                    return true;
                }
                return employment.SalaryFromRange.HasValue;
            }).WithMessage("Enter from range");

            RuleForEach(offerData => offerData.EmploymentTypes).Must(employment =>
            {
                if (!employment.SalaryFromRange.HasValue)
                {
                    return true;
                }
                return employment.SalaryToRange.HasValue;
            }).WithMessage("Enter to range");

            RuleForEach(offerData => offerData.EmploymentTypes).Must(employment =>
            {
                if (!employment.SalaryFromRange.HasValue || !employment.SalaryToRange.HasValue)
                {
                    return true;
                }
                return employment.SalaryCurrencyType.HasValue && employment.SalaryCurrencyType != 0;
            }).WithMessage("Select currency type");
        }

        private void AddApplicationsContactTypeValidationRules()
        {
            RuleFor(offerData => offerData.ApplicationsContactType).NotEqual(0)
                .WithMessage("Select applications contact type");
            When(offerData => offerData.ApplicationsContactType == 1, () =>
            {
                RuleFor(offerData => offerData.ApplicationsContactEmail).NotNull();
                RuleFor(offerData => offerData.ApplicationsContactEmail).EmailAddress();
            });
            When(offerData => offerData.ApplicationsContactType == 2, () =>
            {
                RuleFor(offerData => offerData.ApplicationsContactExternalFormUrl).NotNull();
                RuleFor(offerData => offerData.ApplicationsContactExternalFormUrl!).SetValidator(new UrlValidator());
            });
        }

        private void AddAgreementsValidationRules()
        {
            RuleFor(offerData => offerData.InformationClause).NotEmpty();
            When(offerData => offerData.IsDisplayConsentForFutureRecruitment, () =>
            {
                RuleFor(offerData => offerData.ConsentForFutureRecruitmentContent).NotEmpty()
                    .WithMessage("Enter consent for future recruitment");
            });
            When(offerData => offerData.IsDisplayCustomConsent, () =>
            {
                RuleFor(offerData => offerData.CustomConsentTitle).NotEmpty()
                    .WithMessage("Enter custom consent title");
                RuleFor(offerData => offerData.CustomConsentContent).NotEmpty()
                    .WithMessage("Enter custom consent");
            });
        }

        private string GetInnerTextFromHtml(string htmlText)
        {
            Regex regex = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);
            string resultText = regex.Replace(htmlText, " ").Trim();
            return resultText;
        }
    }
}

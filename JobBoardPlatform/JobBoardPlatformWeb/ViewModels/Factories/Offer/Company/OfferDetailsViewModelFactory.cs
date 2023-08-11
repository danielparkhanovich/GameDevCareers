using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Company
{
    public class OfferDetailsViewModelFactory : IViewModelFactory<JobOffer, OfferDataViewModel>
    {
        public OfferDataViewModel Create(JobOffer offer)
        {
            var viewModel = new OfferDataViewModel()
            {
                OfferId = offer.Id,
                JobTitle = offer.JobTitle,
                City = offer.City,
                Country = offer.Country,
                Street = offer.Address,
                JobDescription = offer.Description,
                MainTechnologyType = offer.MainTechnologyTypeId,
                WorkLocationType = offer.WorkLocationId,
                EmploymentTypes = offer.EmploymentDetails.Select(x => GetEmploymentType(x)).ToArray(),
                TechKeywords = offer.TechKeywords.Select(x => x.Name).ToArray(),
            };

            MapContactType(offer, viewModel);
            MapConsentClauses(offer, viewModel);

            return viewModel;
        }

        private EmploymentTypeViewModel GetEmploymentType(JobOfferEmploymentDetails employment)
        {
            return new EmploymentTypeViewModel()
            {
                SalaryFromRange = (int?)employment.SalaryRange?.From,
                SalaryToRange = (int?)employment.SalaryRange?.To,
                SalaryCurrencyType = employment.SalaryRange?.SalaryCurrencyId,
                TypeId = employment.EmploymentTypeId
            };
        }

        private void MapContactType(JobOffer from, OfferDataViewModel to)
        {
            to.ApplicationsContactType = from.ContactDetails.ContactTypeId;

            if (from.ContactDetails.ContactTypeId == 1)
            {
                to.ApplicationsContactEmail = from.ContactDetails.ContactAddress;
            }
            else if (from.ContactDetails.ContactTypeId == 2)
            {
                to.ApplicationsContactExternalFormUrl = from.ContactDetails.ContactAddress;
            }
        }

        private void MapConsentClauses(JobOffer from, OfferDataViewModel to)
        {
            to.InformationClause = from.InformationClause;

            to.IsDisplayConsentForFutureRecruitment = !string.IsNullOrEmpty(from.ProcessingDataInFutureClause);
            to.ConsentForFutureRecruitmentContent = from.ProcessingDataInFutureClause;

            to.IsDisplayCustomConsent = !string.IsNullOrEmpty(from.CustomConsentClause);
            to.CustomConsentTitle = from.CustomConsentClauseTitle;
            to.CustomConsentContent = from.CustomConsentClause;
        }
    }
}

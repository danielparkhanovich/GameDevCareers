using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Company
{
    public class OfferDetailsViewModelFactory : IViewModelFactory<JobOffer, OfferDetailsViewModel>
    {
        public OfferDetailsViewModel Create(JobOffer offer)
        {
            var viewModel = new OfferDetailsViewModel()
            {
                OfferId = offer.Id,
                JobTitle = offer.JobTitle,
                City = offer.City,
                Country = offer.Country,
                Street = offer.Address,
                JobDescription = offer.Description,
                ApplicationsContactEmail = offer.ContactDetails.ContactAddress,
                ApplicationsContactType = offer.ContactDetails.Id,
                MainTechnologyType = offer.MainTechnologyTypeId,
                WorkLocationType = offer.WorkLocationId,
                SalaryFromRange = offer.JobOfferEmploymentDetails.Select(x => (int?)x.SalaryRange?.From).ToArray(),
                SalaryToRange = offer.JobOfferEmploymentDetails.Select(x => (int?)x.SalaryRange?.To).ToArray(),
                SalaryCurrencyType = offer.JobOfferEmploymentDetails.Select(x => x.SalaryRange.SalaryCurrencyId).ToArray(),
                EmploymentTypes = offer.JobOfferEmploymentDetails.Select(x => x.EmploymentTypeId).ToArray(),
                TechKeywords = offer.TechKeywords.Select(x => x.Name).ToArray(),
            };

            return viewModel;
        }
    }
}

using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.DAL.Data.Loaders;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Company
{
    public class OfferDetailsViewModelFactory : IFactory<OfferDetailsViewModel>
    {
        private readonly int offerId;
        private readonly IRepository<JobOffer> offersRepository;


        public OfferDetailsViewModelFactory(int offerId, IRepository<JobOffer> offersRepository)
        {
            this.offerId = offerId;
            this.offersRepository = offersRepository;
        }

        public async Task<OfferDetailsViewModel> Create()
        {
            var offersLoader = new LoadOfferContent(offersRepository, offerId);
            var offer = await offersLoader.Load();

            var viewModel = new OfferDetailsViewModel()
            {
                OfferId = offerId,
                JobTitle = offer.JobTitle,
                City = offer.City,
                Country = offer.Country,
                Address = offer.Address,
                JobDescription = offer.Description,
                ContactAddress = offer.ContactDetails.ContactAddress,
                ContactType = offer.ContactDetails.Id,
                MainTechnologyType = offer.MainTechnologyTypeId,
                WorkLocationType = offer.WorkLocationId,
                SalaryFromRange = offer.JobOfferEmploymentDetails.Select(x => x.SalaryRange.From).ToArray(),
                SalaryToRange = offer.JobOfferEmploymentDetails.Select(x => x.SalaryRange.To).ToArray(),
                SalaryCurrencyType = offer.JobOfferEmploymentDetails.Select(x => x.SalaryRange.SalaryCurrencyId).ToArray(),
                EmploymentTypes = offer.JobOfferEmploymentDetails.Select(x => x.EmploymentTypeId).ToArray(),
                TechKeywords = offer.TechKeywords.Select(x => x.Name).ToArray(),
            };

            return viewModel;
        }
    }
}

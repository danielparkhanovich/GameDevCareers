using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Offer
{
    public class OfferContentViewModelFactory : IFactory<OfferContentViewModel>
    {
        private readonly int offerId;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IMapper<JobOffer, OfferContentViewModel> offerToContentViewModel;


        public OfferContentViewModelFactory(int offerId, IRepository<JobOffer> offersRepository)
        {
            this.offerId = offerId;
            this.offersRepository = offersRepository;
            this.offerToContentViewModel = new JobOfferToContentViewModelMapper();
        }

        private async Task<JobOffer> GetOffers()
        {
            var offersSet = await offersRepository.GetAllSet();
            // TODO: refactor .Where -> .FindAsync
            var offer = offersSet.Where(x => x.Id == offerId)
                .Include(x => x.CompanyProfile)
                .Include(x => x.WorkLocation)
                .Include(x => x.MainTechnologyType)
                .Include(x => x.TechKeywords)
                .Include(x => x.JobOfferEmploymentDetails)
                    .ThenInclude(y => y.SalaryRange != null ? y.SalaryRange.SalaryCurrency : null)
                .Include(x => x.JobOfferEmploymentDetails)
                    .ThenInclude(y => y.EmploymentType)
                .Include(x => x.ContactDetails)
                    .ThenInclude(y => y.ContactType)
                .Single();

            return offer;
        }

        public async Task<OfferContentViewModel> Create()
        {
            var offer = await GetOffers();

            var display = new OfferContentViewModel();
            offerToContentViewModel.Map(offer, display);

            return display;
        }
    }
}

using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer.CompanyBoard
{
    internal class JobOfferToCompanyOfferViewModelMapper : IMapper<JobOffer, CompanyOfferCardViewModel>
    {
        private readonly IMapper<JobOffer, OfferCardViewModel> jobOfferToOfferCardViewModel;


        public JobOfferToCompanyOfferViewModelMapper()
        {
            this.jobOfferToOfferCardViewModel = new JobOfferToOfferViewModelMapper();
        }

        public void Map(JobOffer from, CompanyOfferCardViewModel to)
        {
            to.TotalViews = from.NumberOfViews;
            to.TotalApplicants = from.NumberOfApplications;
            to.MainTechnology = from.MainTechnologyType.Type;
            to.ContactType = from.ContactDetails.ContactType.Type;
            to.ContactAddress = from.ContactDetails.ContactAddress;

            to.IsPaid = from.IsPaid;
            to.IsPublished = from.IsPublished;
            to.IsShelved = from.IsShelved;
            to.IsDeleted = from.IsDeleted;

            MapCardDisplay(from, to);
        }

        private void MapCardDisplay(JobOffer from, CompanyOfferCardViewModel to)
        {
            var offerCardViewModel = new OfferCardViewModel();

            jobOfferToOfferCardViewModel.Map(from, offerCardViewModel);

            to.CardDisplay = offerCardViewModel;
        }
    }
}

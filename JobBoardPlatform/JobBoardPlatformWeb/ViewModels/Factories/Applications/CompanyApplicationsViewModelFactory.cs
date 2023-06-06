using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.BLL.Search.CompanyPanel.Applications;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationsViewModelFactory : IViewModelAsyncFactory<CompanyApplicationsViewModel>
    {
        private readonly OfferApplicationsSearcher searcher;
        private readonly CompanyPanelApplicationSearchParams searchParams;
        private readonly OfferQueryExecutor queryExecutor;


        public CompanyApplicationsViewModelFactory(
            OfferApplicationsSearcher searcher, 
            CompanyPanelApplicationSearchParams searchParams,
            OfferQueryExecutor queryExecutor)
        {
            this.searcher = searcher;
            this.searchParams = searchParams;
            this.queryExecutor = queryExecutor;
        }

        public async Task<CompanyApplicationsViewModel> CreateAsync()
        {
            var offer = await queryExecutor.GetOfferById(searchParams.OfferId);

            var viewModel = new CompanyApplicationsViewModel();
            viewModel.Applications = await GetApplicationCards();
            viewModel.OfferCard = GetOfferCard(offer);

            viewModel.TotalApplications = offer.NumberOfApplications;
            viewModel.TotalViewsCount = offer.NumberOfViews;

            return viewModel;
        }

        private async Task<CardsContainerViewModel> GetApplicationCards()
        {
            var applicationCardsFactory = new CompanyApplicationsContainerViewModelFactory(
                searcher, searchParams);

            var applicationCards = await applicationCardsFactory.CreateAsync();

            return applicationCards;
        }

        private OfferCardViewModel GetOfferCard(JobOffer offer)
        {
            var offerCardFactory = new OfferCardViewModelFactory();
            var offerCard = offerCardFactory.CreateCard(offer);
            return (offerCard as OfferCardViewModel)!;
        }
    }
}

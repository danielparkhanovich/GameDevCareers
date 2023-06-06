using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Templates
{
    public abstract class CardsContainerViewModelFactoryTemplate<T> : IViewModelAsyncFactory<CardsContainerViewModel>
        where T : IEntity
    {
        public async Task<CardsContainerViewModel> CreateAsync()
        {
            var viewModel = new CardsContainerViewModel()
            {
                Header = GetHeader(),
                ContainerCards = await GetCardsAsync(),
                SearchParams = GetSearchParams(),
                RecordsCount = GetTotalRecordsCount()
            };
            return viewModel;
        }

        protected abstract ContainerHeaderViewModel? GetHeader();

        protected abstract IPageSearchParams GetSearchParams();

        protected abstract int GetTotalRecordsCount();

        protected abstract Task<List<IContainerCard>> GetCardsAsync();

        protected List<IContainerCard> GetCards(IContainerCardFactory<T> cardFactory, List<T> entities)
        {
            var cards = new List<IContainerCard>();
            foreach (var entity in entities)
            {
                var card = cardFactory.CreateCard(entity);
                cards.Add(card);
            }
            return cards;
        }
    }
}

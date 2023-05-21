using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Templates;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Templates
{
    public class CardsContainerViewModelFactory<TCardFactory, TCardEntity> : IFactory<CardsContainerViewModel>
        where TCardFactory: IViewModelFactory<TCardEntity, IContainerCard>, new()
        where TCardEntity: IEntity
    {
        private readonly List<TCardEntity> entities;
        private readonly ContainerHeaderViewModel? header;
        private readonly ISearchParameters searchData;
        private readonly string cardPartialViewName;
        private readonly int afterFiltersCount;


        public CardsContainerViewModelFactory(List<TCardEntity> entities,
            ContainerHeaderViewModel? header,
            ISearchParameters searchData,
            string cardPartialViewName,
            int afterFiltersCount)
        {
            this.entities = entities;
            this.header = header;
            this.searchData = searchData;
            this.cardPartialViewName = cardPartialViewName;
            this.afterFiltersCount = afterFiltersCount;
        }

        public Task<CardsContainerViewModel> Create()
        {
            var cards = new List<IContainerCard>();
            foreach (var entity in entities)
            {
                var cardFactory = new TCardFactory();
                var card = cardFactory.CreateViewModel(entity);

                cards.Add(card);
            }

            var viewModel = new CardsContainerViewModel()
            {
                CardPartialViewModelName = cardPartialViewName,
                Header = header,
                ContainerCards = cards,
                SearchParams = searchData,
                RecordsCount = afterFiltersCount
            };

            return Task.FromResult(viewModel);
        }
    }
}

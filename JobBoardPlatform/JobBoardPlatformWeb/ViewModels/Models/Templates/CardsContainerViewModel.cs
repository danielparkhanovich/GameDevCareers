using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.PL.ViewModels.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Templates
{
    public class CardsContainerViewModel
    {
        public ContainerHeaderViewModel? Header { get; set; }
        public ISearchParameters SearchParams { get; set; }

        // e.g. ./JobOffers/_ApplicationCard or nameof(_ApplicationCard)
        public string CardPartialViewModelName { get; set; }
        public ICollection<IContainerCard>? ContainerCards { get; set; }

        public int RecordsCount { get; set; }
    }
}

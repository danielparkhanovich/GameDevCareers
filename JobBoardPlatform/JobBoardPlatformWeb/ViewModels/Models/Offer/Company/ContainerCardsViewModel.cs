using JobBoardPlatform.DAL.Data.Enums.Sort;
using JobBoardPlatform.PL.ViewModels.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company
{
    public class ContainerCardsViewModel
    {
        public int RelatedId { get; set; }
        public ICollection<IContainerCard>? ContainerCards { get; set; }
        public int Page { get; set; }
        public SortCategoryType SortCategory { get; set; }
        public SortCategoryType[] SortCategoryTypes { get; set; }
        public SortType SortType { get; set; }
        public bool[] FilterToggles { get; set; }
        public int RecordsCount { get; set; }

        // e.g. ./JobOffers/_ApplicationCard or nameof(_ApplicationCard)
        public string CardPartialViewModelName { get; set; }
        public string[] SortLabels { get; set; }
        public string[] FilterLabels { get; set; }
        public bool IsShowHeader { get; set; } = true;
    }
}

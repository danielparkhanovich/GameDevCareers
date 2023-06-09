using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Company
{
    public class CompanyOfferHeaderViewModelFactory
    {
        public ContainerHeaderViewModel CreateViewModel()
        {
            var filter = new (string, string)[]
            {
                ("Published", OfferSearchUrlParams.HidePublished),
                ("Shelved", OfferSearchUrlParams.HideShelved),
            };

            var sort = new (string, string)[]
            {
                ("Date", SortCategoryType.PublishDate.ToString()),
                ("Alphabetically", SortCategoryType.Alphabetically.ToString()),
                ("Relevenacy", SortCategoryType.Relevenacy.ToString())
            };

            var viewModel = new ContainerHeaderViewModel()
            {
                FilterLabels = filter.Select(x => x.Item1).ToArray(),
                FilterValues = filter.Select(x => x.Item2).ToArray(),
                SortLabels = sort.Select(x => x.Item1).ToArray(),
                SortValues = sort.Select(x => x.Item2).ToArray(),
                IsInvertFilters = true,
            };

            return viewModel;
        }
    }
}

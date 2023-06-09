using JobBoardPlatform.BLL.Search;
using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Applications
{
    public class CompanyApplicationsHeaderViewModelFactory
    {
        public ContainerHeaderViewModel CreateViewModel()
        {
            var filter = new (string, string)[]
            {
                ("Unseen", OfferSearchUrlParams.HideUnseen),
                ("<i class=\"bi bi-star\"></i>", OfferSearchUrlParams.HideMustHire),
                ("<i class=\"bi bi-question-lg\"></i>", OfferSearchUrlParams.HideAverage),
                ("<i class=\"bi bi-x-lg\"></i>", OfferSearchUrlParams.HideRejected)
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

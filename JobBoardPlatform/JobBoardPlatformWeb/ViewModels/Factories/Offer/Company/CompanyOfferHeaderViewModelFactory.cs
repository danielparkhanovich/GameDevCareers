using JobBoardPlatform.BLL.Search.Enums;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer.Company
{
    public class CompanyOfferHeaderViewModelFactory
    {
        public ContainerHeaderViewModel CreateViewModel()
        {
            var filterLabels = new string[]
            {
                "Published",
                "Shelved"
            };

            var sort = new (string, string)[]
            {
                ("Date", SortCategoryType.PublishDate.ToString()),
                ("Alphabetically", SortCategoryType.Alphabetically.ToString()),
                ("Relevenacy", SortCategoryType.Relevenacy.ToString())
            };
            var sortLabels = sort.Select(x => x.Item1).ToArray();
            var sortValues = sort.Select(x => x.Item2).ToArray();

            var viewModel = new ContainerHeaderViewModel()
            {
                FilterLabels = filterLabels,
                SortLabels = sortLabels,
                SortValues = sortValues,
            };

            return viewModel;
        }
    }
}

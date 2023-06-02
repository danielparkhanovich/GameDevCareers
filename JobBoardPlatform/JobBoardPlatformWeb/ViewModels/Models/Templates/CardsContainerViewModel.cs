using JobBoardPlatform.BLL.Search.Contracts;
using JobBoardPlatform.PL.ViewModels.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Templates
{
    public class CardsContainerViewModel
    {
        public const string PartialView = "./Templates/_CardsContainer";

        public ContainerHeaderViewModel? Header { get; set; }
        public IPageSearchParams SearchParams { get; set; }
        public ICollection<IContainerCard>? ContainerCards { get; set; }
        public int RecordsCount { get; set; }
    }
}

using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Models.Admin
{
    public class AdminPanelOffersViewModel : AdminPanelViewModel<CompanyProfile>
    {
        public CardsContainerViewModel OffersContainer { get; set; }
        public List<CompanyProfile> AllRecords { get; set; }
        public int CountToGenerate { get; set; }
        public int CompanyIdToGenerate { get; set; }
    }
}

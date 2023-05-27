using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Models.Admin
{
    public class AdminPanelCompaniesViewModel : AdminPanelViewModel<CompanyIdentity>
    {
        public CardsContainerViewModel OffersContainer { get; set; }
        public List<CompanyIdentity> AllRecords { get; set; }
        public int CountToGenerate { get; set; }
    }
}

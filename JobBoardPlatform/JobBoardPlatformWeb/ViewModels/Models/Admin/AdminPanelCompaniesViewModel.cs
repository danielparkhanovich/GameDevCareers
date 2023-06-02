using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Models.Admin
{
    public class AdminPanelCompaniesViewModel : IAdminPanelViewModel<CompanyIdentity>
    {
        public CardsContainerViewModel CardsContainer { get; set; }
        public List<CompanyIdentity> AllRecords { get; set; }
        public int CountToGenerate { get; set; }
    }
}

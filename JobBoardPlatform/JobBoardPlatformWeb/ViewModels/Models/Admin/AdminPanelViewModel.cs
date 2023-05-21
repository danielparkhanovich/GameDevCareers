using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Models.Admin
{
    public class AdminPanelViewModel
    {
        public CardsContainerViewModel OffersContainer { get; set; }
        public List<CompanyProfile> AllCompanies { get; set; }
        public int OffersCountToGenerate { get; set; }
        public int CompanyIdToGenerate { get; set; }
    }
}

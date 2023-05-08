using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;

namespace JobBoardPlatform.PL.ViewModels.Models.Admin
{
    public class AdminPanelViewModel
    {
        public ContainerCardsViewModel OffersContainer { get; set; }
        public List<CompanyProfile> AllCompanies { get; set; }
        public int OffersCountToGenerate { get; set; }
        public int CompanyIdToGenerate { get; set; }
    }
}

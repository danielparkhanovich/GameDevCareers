using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company
{
    public class JobOffersDisplayCompanyViewModel
    {
        public ICollection<JobOfferCardDisplayCompanyViewModel>? JobOffers { get; set; }
    }
}

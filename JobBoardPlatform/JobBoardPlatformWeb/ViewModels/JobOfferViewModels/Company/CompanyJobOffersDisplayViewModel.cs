using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company
{
    public class CompanyJobOffersDisplayViewModel
    {
        public ICollection<JobOfferCardDisplayViewModel>? JobOffers { get; set; }
    }
}

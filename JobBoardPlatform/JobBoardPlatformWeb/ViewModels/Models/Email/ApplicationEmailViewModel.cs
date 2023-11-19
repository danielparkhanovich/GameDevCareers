using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;

namespace JobBoardPlatform.PL.ViewModels.Models.Email
{
    public class ApplicationEmailViewModel
    {
        public const string EmailView = "./Email/_ApplicationForm";


        public string CompanyName { get; set; } = string.Empty;
        public string JobTitle { get;set; } = string.Empty;
        public ApplicationCardViewModel Application { get; set; } = new ApplicationCardViewModel();
    }
}

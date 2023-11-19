using JobBoardPlatform.PL.ViewModels.Models.Profile.Employee;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Users
{
    public class OfferContentViewModel
    {
        public OfferContentDisplayViewModel Display { get; set; }
        public OfferApplicationUpdateViewModel Update { get; set; }


        public OfferContentViewModel()
        {
            Display = new OfferContentDisplayViewModel();
            Update = new OfferApplicationUpdateViewModel()
            {
                AttachedResume = new EmployeeAttachedResumeViewModel()
            };
        }
    }
}

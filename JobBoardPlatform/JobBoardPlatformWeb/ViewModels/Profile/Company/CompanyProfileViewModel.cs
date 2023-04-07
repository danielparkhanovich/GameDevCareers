using JobBoardPlatform.PL.ViewModels.Profile.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Profile.Company
{
    public class CompanyProfileViewModel : IProfileViewModel
    {
        public string? ProfileImageUrl { get => Display.ProfileImageUrl; }
        public IFormFile? ProfileImage { get => Update.ProfileImage; set => Update.ProfileImage = value; }

        public CompanyProfileDisplayViewModel Display { get; set; }
        public CompanyProfileUpdateViewModel Update { get; set; }


        public CompanyProfileViewModel()
        {
            Display = new CompanyProfileDisplayViewModel();
            Update = new CompanyProfileUpdateViewModel();
        }
    }
}

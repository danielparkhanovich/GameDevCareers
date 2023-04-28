using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Profile.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Profile
{
    public class CompanyProfileViewModelFactory : IViewModelFactory<CompanyProfile, CompanyProfileViewModel>
    {
        public CompanyProfileViewModel CreateViewModel(CompanyProfile profile)
        {
            var viewModel = new CompanyProfileViewModel()
            {
                CompanyName = profile.CompanyName,
                City = profile.City,
                Country = profile.Country,
                ProfileImageUrl = profile.ProfileImageUrl,
                CompanyWebsiteUrl = profile.CompanyWebsiteUrl
            };

            return viewModel;
        }
    }
}

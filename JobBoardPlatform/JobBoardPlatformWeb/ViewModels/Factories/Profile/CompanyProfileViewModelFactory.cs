using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Company;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Profile
{
    public class CompanyProfileViewModelFactory : IViewModelFactory<CompanyProfile, CompanyProfileViewModel>
    {
        public CompanyProfileViewModel Create(CompanyProfile profile)
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

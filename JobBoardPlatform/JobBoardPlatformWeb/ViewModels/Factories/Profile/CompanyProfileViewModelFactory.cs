using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Common;
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
                OfficeCity = profile.City,
                OfficeCountry = profile.Country,
                ProfileImage = GetProfileImage(profile.ProfileImageUrl),
                CompanyWebsiteUrl = profile.CompanyWebsiteUrl
            };

            return viewModel;
        }

        private ProfileImage GetProfileImage(string? imageUrl) 
        {
            return new ProfileImageViewModel()
            {
                ImageUrl = imageUrl
            };
        }
    }
}

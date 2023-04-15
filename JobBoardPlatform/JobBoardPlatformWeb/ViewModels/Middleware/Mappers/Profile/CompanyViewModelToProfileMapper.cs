using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Profile.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Utilities.Mappers.Profile
{
    internal class CompanyViewModelToProfileMapper : IMapper<CompanyProfileViewModel, CompanyProfile>
    {
        public void Map(CompanyProfileViewModel from, CompanyProfile to)
        {
            var fromUpdate = from.Update;

            if (!string.IsNullOrEmpty(fromUpdate.CompanyName))
            {
                to.CompanyName = fromUpdate.CompanyName;
            }
            if (!string.IsNullOrEmpty(fromUpdate.ProfileImageUrl))
            {
                to.ProfileImageUrl = fromUpdate.ProfileImageUrl;
            }

            to.City = fromUpdate.City;
            to.Country = fromUpdate.Country;
            to.CompanyWebsiteUrl = fromUpdate.CompanyWebsiteUrl;
        }
    }
}

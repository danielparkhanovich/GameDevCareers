using JobBoardPlatform.BLL.Services.Utilities.Contracts;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.PL.ViewModels.Profile;

namespace JobBoardPlatform.BLL.Services.Utilities
{
    internal class CompanyViewModelToProfileMapper : IMapper<CompanyProfileViewModel, CompanyProfile>
    {
        public void Map(CompanyProfileViewModel from, CompanyProfile to)
        {
            if (!string.IsNullOrEmpty(from.CompanyName))
            {
                to.CompanyName = from.CompanyName;
            }
            if (!string.IsNullOrEmpty(from.City))
            {
                to.City = from.City;
            }
            if (!string.IsNullOrEmpty(from.Country))
            {
                to.Country = from.Country;
            }
            if (!string.IsNullOrEmpty(from.PhotoUrl))
            {
                to.ProfileImageUrl = from.PhotoUrl;
            }
        }
    }
}

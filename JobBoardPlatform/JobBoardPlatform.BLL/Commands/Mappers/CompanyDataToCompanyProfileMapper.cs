using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Mappers
{
    public class CompanyDataToCompanyProfileMapper : IMapper<CompanyProfileData, CompanyProfile>
    {
        public void Map(CompanyProfileData from, CompanyProfile to)
        {
            if (!string.IsNullOrEmpty(from.CompanyName))
            {
                to.CompanyName = from.CompanyName;
            }
            if (!string.IsNullOrEmpty(from.ProfileImage.ImageUrl))
            {
                to.ProfileImageUrl = from.ProfileImage.ImageUrl;
            }

            to.City = from.OfficeCity;
            to.Country = from.OfficeCountry;
            to.CompanyWebsiteUrl = from.CompanyWebsiteUrl;
        }
    }
}

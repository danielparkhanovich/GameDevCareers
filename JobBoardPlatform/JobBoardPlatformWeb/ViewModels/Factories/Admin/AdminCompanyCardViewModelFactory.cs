using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.Controllers.Utils;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Admin;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminCompanyCardViewModelFactory : IContainerCardFactory<CompanyIdentity>
    {
        public IContainerCard CreateCard(CompanyIdentity company)
        {
            var card = new AdminCompanyCardViewModel();

            card.Id = company.Id;
            card.CompanyName = company.Profile.CompanyName;
            card.Email = company.Email;
            card.Country = company.Profile.Country;
            card.City = company.Profile.City;
            card.ProfileImageUrl = GetProfileImageUri(company);
            card.CompanyWebsiteUrl = company.Profile.CompanyWebsiteUrl;

            return card;
        }

        private string GetProfileImageUri(CompanyIdentity company)
        {
            return StaticFilesUtils.GetCompanyDefaultAvatarUriIfEmpty(company.Profile.ProfileImageUrl);
        }
    }
}

using JobBoardPlatform.BLL.Common.Formatter;
using JobBoardPlatform.BLL.Services.Background;
using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Offer;
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
            card.Country = company.Profile.Country;
            card.City = company.Profile.City;
            card.ProfileImageUrl = company.Profile.ProfileImageUrl;
            card.CompanyWebsiteUrl = company.Profile.CompanyWebsiteUrl;

            return card;
        }
    }
}

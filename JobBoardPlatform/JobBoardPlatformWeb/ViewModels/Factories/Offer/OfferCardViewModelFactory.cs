using JobBoardPlatform.BLL.Common.Formatter;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    internal class OfferCardViewModelFactory : IContainerCardFactory<JobOffer>
    {
        public IContainerCard CreateCard(JobOffer offer)
        {
            var offerCardViewModel = new OfferCardViewModel();

            Map(offer, offerCardViewModel);

            return offerCardViewModel;
        }

        public void Map(JobOffer from, OfferCardViewModel to)
        {
            to.Id = from.Id;
            to.JobTitle = from.JobTitle;
            to.Company = from.CompanyProfile.CompanyName;
            to.CompanyImageUrl = from.CompanyProfile.ProfileImageUrl;
            to.Country = from.Country;
            to.City = from.City;
            to.WorkLocationType = from.WorkLocation.Type;
            to.SalaryDetails = GetSalaryDetails(from);
            to.PublishedAgo = GetPublishedAgo(from);
            to.TechKeywords = GetKeywords(from);
        }

        private string GetSalaryDetails(JobOffer from)
        {
            var salaryFormatter = new SalaryOnCardFormatter();
            return salaryFormatter.GetString(from);
        }

        private string GetPublishedAgo(JobOffer from)
        {
            var daysFormatter = new OfferPublishedAgoFormatter();
            return daysFormatter.GetString(from);
        }

        private string[]? GetKeywords(JobOffer from)
        {
            return from.TechKeywords.Select(x => x.Name).ToArray();
        }
    }
}

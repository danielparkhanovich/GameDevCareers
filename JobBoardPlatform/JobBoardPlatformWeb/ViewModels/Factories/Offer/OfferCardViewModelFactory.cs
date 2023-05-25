using JobBoardPlatform.BLL.Common.Formatter;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    internal class OfferCardViewModelFactory : IViewModelFactory<JobOffer, IContainerCard>
    {
        public IContainerCard CreateViewModel(JobOffer offer)
        {
            var offerCardViewModel = new OfferCardViewModel();

            Map(offer, offerCardViewModel);

            return offerCardViewModel;
        }

        public void Map(JobOffer from, OfferCardViewModel to)
        {
            var salaryFormatter = new SalaryFormatter();
            string salaryDetails = salaryFormatter.GetString(from);

            var daysFormatter = new PublishedAgoFormatter(from.IsPublished);
            string publishedAgo = daysFormatter.GetString(from.PublishedAt);

            var techKeywords = from.TechKeywords.Select(x => x.Name).ToArray();

            to.Id = from.Id;
            to.JobTitle = from.JobTitle;
            to.Company = from.CompanyProfile.CompanyName;
            to.CompanyImageUrl = from.CompanyProfile.ProfileImageUrl;
            to.Country = from.Country;
            to.City = from.City;
            to.WorkLocationType = from.WorkLocation.Type;
            to.SalaryDetails = salaryDetails;
            to.PublishedAgo = publishedAgo;
            to.TechKeywords = techKeywords;
        }
    }
}

using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer
{
    internal class JobOfferToOfferViewModelMapper : IMapper<JobOffer, OfferCardViewModel>
    {
        public void Map(JobOffer from, OfferCardViewModel to)
        {
            string salaryDetails = GetSalaryString(from);

            string publishedAgo = GetPublishedAgoString(from);

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

        private string GetSalaryString(JobOffer from)
        {
            string salaryDetails = "Undisclosed Salary";

            if (from.JobOfferEmploymentDetails != null && from.JobOfferEmploymentDetails.Count > 0)
            {
                var details = from.JobOfferEmploymentDetails.Where(x => x.SalaryRange != null);

                if (details != null && details.Count() > 0)
                {
                    var firstDetails = details.OrderBy(x => x.SalaryRange!.To).First();
                    var salary = firstDetails.SalaryRange!;
                    salaryDetails = $"{salary.From} - {salary.To} {salary.SalaryCurrency.Type}";
                }
            }

            return salaryDetails;
        }

        private string GetPublishedAgoString(JobOffer from)
        {
            string publishedAgo = $"0d ago";

            if (from.IsPublished)
            {
                publishedAgo = $"{(DateTime.Now - from.PublishedAt).Days}d ago";
            }

            return publishedAgo;
        }
    }
}

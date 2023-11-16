using JobBoardPlatform.BLL.Common.Formatter;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.MainTechnologyWidgets;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Users;

namespace JobBoardPlatform.PL.ViewModels.Factories.Offer
{
    public class OfferContentDisplayViewModelFactory : IViewModelFactory<JobOffer, OfferContentDisplayViewModel>
    {
        public OfferContentDisplayViewModel Create(JobOffer offer)
        {
            var viewModel = new OfferContentDisplayViewModel();

            Map(offer, viewModel);

            return viewModel;
        }

        public void Map(JobOffer from, OfferContentDisplayViewModel to)
        {
            var techKeyWords = from.TechKeywords.Select(x => x.Name).ToArray();

            to.TechKeywords = techKeyWords;
            to.JobTitle = from.JobTitle;
            to.JobDescription = from.Description;
            to.CompanyName = from.CompanyProfile.CompanyName;
            to.CompanyImageUrl = from.CompanyProfile.ProfileImageUrl;
            to.CompanyWebsiteUrl = from.CompanyProfile.CompanyWebsiteUrl;
            to.WorkLocationType = from.WorkLocation.Type;
            to.WorkLocationTypeText = GetWorkLocationText(from);
            to.ContactForm = from.ContactDetails.ContactType.Type;
            to.ExternalFormUrl = from.ContactDetails.ContactAddress;
            to.OfferCategory = from.Plan.Category.Type;

            MapMainTechnologyWidget(from, to);
            MapPublishedAgo(from, to);
            MapInformationClauses(from, to);
            MapSalaryDetails(from, to);
            MapFullAddress(from, to);
        }

        private void MapInformationClauses(JobOffer from, OfferContentDisplayViewModel to)
        {
            to.InformationClause = from.InformationClause;
            to.ProcessingDataInFutureClause = from.ProcessingDataInFutureClause;
            to.CustomConsentClauseTitle = from.CustomConsentClauseTitle;
            to.CustomConsentClause = from.CustomConsentClause;
        }

        private void MapSalaryDetails(JobOffer from, OfferContentDisplayViewModel to)
        {
            var salaryDisplayText = new List<string>(from.EmploymentDetails.Count);
            var employmentTypeDisplayText = new List<string>(from.EmploymentDetails.Count);

            var salaryFormatter = new SalaryFormatter();
            var employmentTypeFormatter = new EmploymentTypeFormatter();
            foreach (var employmentDetails in from.EmploymentDetails)
            {
                string singleSalaryText = salaryFormatter.GetString(employmentDetails.SalaryRange);
                string employmentType = employmentTypeFormatter.GetString(employmentDetails.EmploymentType.Type);

                salaryDisplayText.Add(singleSalaryText);
                employmentTypeDisplayText.Add(employmentType);
            }

            to.SalaryDetails = salaryDisplayText.ToArray();
            to.EmploymentDetails = employmentTypeDisplayText.ToArray();
        }

        private void MapFullAddress(JobOffer from, OfferContentDisplayViewModel to)
        {
            string fullAddress = $"{from.City}, {from.Country}";
            if (!string.IsNullOrEmpty(from.Address))
            {
                fullAddress = $"{from.Address}, {fullAddress}";
            }

            to.FullAddress = fullAddress;
        }

        private void MapPublishedAgo(JobOffer from, OfferContentDisplayViewModel to)
        {
            var daysFormatter = new OfferPublishedAgoFormatter();
            to.PublishedAgo = daysFormatter.GetString(from);
        }

        private void MapMainTechnologyWidget(JobOffer from, OfferContentDisplayViewModel to)
        {
            var factory = new MainTechnologyWidgetFactory();
            to.MainTechnologyWidget = factory.Create(from.MainTechnologyType.Type);
        }

        private string GetWorkLocationText(JobOffer from)
        {
            var formatter = new WorkLocationTypeFormatter();
            return formatter.GetString(from.WorkLocation.Type);
        }
    }
}

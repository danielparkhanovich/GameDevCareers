using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.EnumTables;
using JobBoardPlatform.PL.ViewModels.Offer.Company.Contracts;
using JobBoardPlatform.PL.ViewModels.Offer.Users;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using System;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer
{
    internal class JobOfferToContentViewModelMapper : IMapper<JobOffer, OfferContentViewModel>
    {
        public void Map(JobOffer from, OfferContentViewModel to)
        {
            var techKeyWords = from.TechKeywords.Select(x => x.Name).ToArray();

            to.TechKeywords = techKeyWords;

            to.JobTitle = from.JobTitle;
            to.JobDescription = from.Description;
            to.CompanyName = from.CompanyProfile.CompanyName;
            to.CompanyImageUrl = from.CompanyProfile.ProfileImageUrl;
            to.MainTechnologyType = from.MainTechnologyType.Type;
            to.WorkLocationType = from.WorkLocation.Type;
            to.ContactForm = from.ContactDetails.ContactType.Type;

            MapSalaryDetails(from, to);
            MapFullAddress(from, to);
        }

        private void MapSalaryDetails(JobOffer from, OfferContentViewModel to)
        {
            var salaryDisplayText = new List<string>(from.JobOfferEmploymentDetails.Count);
            var employmentTypeDisplayText = new List<string>(from.JobOfferEmploymentDetails.Count);

            foreach (var employmentDetails in from.JobOfferEmploymentDetails)
            {
                string singleSalaryText = "Undisclosed Salary";

                var salaryRange = employmentDetails.SalaryRange;

                if (salaryRange != null)
                {
                    singleSalaryText = $"{salaryRange.From} - {salaryRange.To} {salaryRange.SalaryCurrency.Type}";
                }

                string employmentType = employmentDetails.EmploymentType.Type;

                salaryDisplayText.Add(singleSalaryText);
                employmentTypeDisplayText.Add(employmentType);
            }

            to.SalaryDetails = salaryDisplayText.ToArray();
            to.EmploymentDetails = employmentTypeDisplayText.ToArray();
        }

        private void MapFullAddress(JobOffer from, OfferContentViewModel to)
        {
            string fullAddress = $"{from.City}, {from.Country}";
            if (!string.IsNullOrEmpty(from.Address))
            {
                fullAddress = $"{from.Address}, {fullAddress}";
            }

            to.FullAddress = fullAddress;
        }
    }
}

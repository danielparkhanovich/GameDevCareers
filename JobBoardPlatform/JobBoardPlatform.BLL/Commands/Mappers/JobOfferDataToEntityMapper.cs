using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Commands.Mappers
{
    public class JobOfferDataToEntityMapper : IMapper<IOfferData, JobOffer>
    {
        public void Map(IOfferData from, JobOffer to)
        {
            to.JobTitle = from.JobTitle;
            to.Description = from.JobDescription;
            to.Country = from.Country;
            to.City = from.City;
            to.Address = from.Street;

            to.WorkLocationId = from.WorkLocationType;
            to.MainTechnologyTypeId = from.MainTechnologyType;

            MapContactDetails(from, to);

            if (from.EmploymentTypes != null)
            {
                MapOfferDetails(from, to);
            }

            if (from.TechKeywords != null)
            {
                MapTechKeywords(from, to);
            }
        }

        private void MapContactDetails(IOfferData from, JobOffer to)
        {
            var contactDetails = new JobOfferContactDetails();
            contactDetails.ContactTypeId = from.ApplicationsContactType;

            // ApplicationsContactEmailAddress is null in case of private messages on the website
            contactDetails.ContactAddress = from.ApplicationsContactEmail;

            to.ContactDetails = contactDetails;
        }

        private void MapOfferDetails(IOfferData from, JobOffer to)
        {
            var employmentDetails = new JobOfferEmploymentDetails[from.EmploymentTypes.Length];

            for (int i = 0; i < from.EmploymentTypes.Length; i++)
            {
                int employmentTypeId = from.EmploymentTypes[i];

                employmentDetails[i] = new JobOfferEmploymentDetails();
                employmentDetails[i].EmploymentTypeId = employmentTypeId;

                int? currencyTypeId = from.SalaryCurrencyType?[i] ?? null;
                int? salaryFrom = from.SalaryFromRange?[i] ?? null;
                int? salaryTo = from.SalaryToRange?[i] ?? null;

                if (currencyTypeId.HasValue && salaryFrom.HasValue && salaryTo.HasValue)
                {
                    employmentDetails[i].SalaryRange = new JobOfferSalariesRange()
                    {
                        From = salaryFrom.Value,
                        To = salaryTo.Value,
                        SalaryCurrencyId = currencyTypeId.Value
                    };
                }
            }
            to.EmploymentDetails = employmentDetails;
        }

        private void MapTechKeywords(IOfferData from, JobOffer to)
        {
            from.TechKeywords = from.TechKeywords!.Distinct().ToArray();

            var techKeywords = new List<JobOfferTechKeyword>(from.TechKeywords.Length);

            for (int i = 0; i < from.TechKeywords.Length; i++)
            {
                string keywordString = from.TechKeywords[i];

                if (string.IsNullOrEmpty(keywordString))
                {
                    continue;
                }

                var keyword = new JobOfferTechKeyword() 
                { 
                    Name = keywordString 
                };

                techKeywords.Add(keyword);
            }

            if (techKeywords.Count > 0)
            {
                to.TechKeywords = techKeywords;
            }
        }
    }
}

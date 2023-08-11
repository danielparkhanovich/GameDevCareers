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

            MapConsentClauses(from, to);

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
            if (contactDetails.ContactTypeId == 1)
            {
                contactDetails.ContactAddress = from.ApplicationsContactEmail;
            }
            else if (contactDetails.ContactTypeId == 2)
            {
                contactDetails.ContactAddress = from.ApplicationsContactExternalFormUrl;
            }
            // ContactTypeId == 3 / ApplicationsContactEmailAddress is null in case of private messages on the website

            to.ContactDetails = contactDetails;
        }

        private void MapOfferDetails(IOfferData from, JobOffer to)
        {
            var employmentDetails = new JobOfferEmploymentDetails[from.EmploymentTypes.Length];

            for (int i = 0; i < from.EmploymentTypes.Length; i++)
            {
                var employment = from.EmploymentTypes[i];

                employmentDetails[i] = new JobOfferEmploymentDetails();
                employmentDetails[i].EmploymentTypeId = employment.TypeId;
                MapSalaryRange(employment, employmentDetails[i]);
            }
            to.EmploymentDetails = employmentDetails;
        }

        private void MapSalaryRange(EmploymentType employment, JobOfferEmploymentDetails model)
        {
            int? currencyTypeId = employment.SalaryCurrencyType ?? null;
            int? salaryFrom = employment.SalaryFromRange ?? null;
            int? salaryTo = employment.SalaryToRange ?? null;

            if (currencyTypeId.HasValue && salaryFrom.HasValue && salaryTo.HasValue)
            {
                model.SalaryRange = new JobOfferSalariesRange()
                {
                    From = salaryFrom.Value,
                    To = salaryTo.Value,
                    SalaryCurrencyId = currencyTypeId.Value
                };
            }
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

        private void MapConsentClauses(IOfferData from, JobOffer to)
        {
            to.InformationClause = from.InformationClause;

            if (from.IsDisplayConsentForFutureRecruitment)
            {
                to.ProcessingDataInFutureClause = from.ConsentForFutureRecruitmentContent;
            }
            if (from.IsDisplayCustomConsent)
            {
                to.CustomConsentClauseTitle = from.CustomConsentTitle;
                to.CustomConsentClause = from.CustomConsentContent;
            }
        }
    }
}

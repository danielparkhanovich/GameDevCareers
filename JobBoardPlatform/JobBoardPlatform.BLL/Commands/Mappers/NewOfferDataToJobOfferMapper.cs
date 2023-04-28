using JobBoardPlatform.BLL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Mappers
{
    internal class NewOfferDataToJobOfferMapper : IMapper<INewOfferData, JobOffer>
    {
        private readonly IRepository<TechKeyword> keyWordsRepository;


        public NewOfferDataToJobOfferMapper(IRepository<TechKeyword> keyWordsRepository)
        {
            this.keyWordsRepository = keyWordsRepository;
        }

        public void Map(INewOfferData from, JobOffer to)
        {
            to.JobTitle = from.JobTitle;
            to.Description = from.JobDescription;
            to.Country = from.Country;
            to.City = from.City;
            to.Address = from.Address;

            to.WorkLocationId = from.WorkLocationType;
            to.MainTechnologyTypeId = from.MainTechnologyType;

            MapContactDetails(from, to);

            MapOfferDetails(from, to);

            if (from.TechKeywords != null)
            {
                MapTechKeywords(from, to);
            }
        }

        private void MapContactDetails(INewOfferData from, JobOffer to)
        {
            var contactDetails = new ContactDetails();
            contactDetails.ContactTypeId = from.ContactType;

            // ContactAddress is null in case of private messages on the website
            contactDetails.ContactAddress = from.ContactAddress;

            to.ContactDetails = contactDetails;
        }

        private void MapOfferDetails(INewOfferData from, JobOffer to)
        {
            var employmentDetails = new JobOfferEmploymentDetails[from.EmploymentTypes.Length];

            for (int i = 0; i < from.EmploymentTypes.Length; i++)
            {
                int employmentTypeId = from.EmploymentTypes[i];

                employmentDetails[i] = new JobOfferEmploymentDetails();
                employmentDetails[i].EmploymentTypeId = employmentTypeId;

                int? currencyTypeId = from.SalaryCurrencyType[i];
                int? salaryFrom = from.SalaryFromRange[i];
                int? salaryTo = from.SalaryToRange[i];

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

            to.JobOfferEmploymentDetails = employmentDetails;
        }

        private async void MapTechKeywords(INewOfferData from, JobOffer to)
        {
            from.TechKeywords = from.TechKeywords.Distinct().ToArray();

            var techKeywords = new List<TechKeyword>(from.TechKeywords.Length);

            for (int i = 0; i < from.TechKeywords.Length; i++)
            {
                string keywordString = from.TechKeywords[i];

                if (string.IsNullOrEmpty(keywordString))
                {
                    continue;
                }

                var keywordsSet = await keyWordsRepository.GetAllSet();
                if (keywordsSet.Any(x => x.Name == keywordString))
                {
                    continue;
                }

                var keyword = new TechKeyword() { Name = keywordString };

                techKeywords.Add(keyword);
            }

            if (techKeywords.Count > 0)
            {
                to.TechKeywords = techKeywords;
            }
        }
    }
}

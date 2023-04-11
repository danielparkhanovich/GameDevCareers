using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Utilities
{
    internal class JobOfferViewModelToJobOfferMapper : IMapper<JobOfferUpdateViewModel, JobOffer>
    {
        public void Map(JobOfferUpdateViewModel from, JobOffer to)
        {
            to.JobTitle = from.JobTitle;
            to.Description = from.JobDescription;
            to.Country = from.Country;
            to.City = from.City;
            to.Address = from.Address;

            to.WorkLocationId = from.WorkLocationType;
            to.MainTechnologyTypeId = from.MainTechnology;

            MapOfferDetails(from, to);

            if (from.TechKeyWords != null)
            {
                MapTechKeyWords(from, to);
            }
        }

        private void MapOfferDetails(JobOfferUpdateViewModel from, JobOffer to)
        {
            var employmentDetails = new JobOfferEmploymentDetails[from.EmploymentTypes.Length];

            for (int i = 0; i < from.EmploymentTypes.Length; i++)
            {
                var employmentType = from.EmploymentTypes[i];
                var employmentTypeEnum = (EmploymentTypeEnum)Enum.Parse(typeof(EmploymentTypeEnum), employmentType);
                int employmentTypeId = Array.IndexOf(
                    Enum.GetValues(typeof(EmploymentTypeEnum)).Cast<EmploymentTypeEnum>().ToArray(), employmentTypeEnum);
                employmentTypeId += 1;

                var currency = from.SalaryCurrency[i];
                var salaryFrom = from.SalaryFromRange[i];
                var salaryTo = from.SalaryToRange[i];

                var currencyTypeEnum = (CurrencyTypeEnum)Enum.Parse(typeof(CurrencyTypeEnum), currency);
                int currencyTypeId = Array.IndexOf(
                    Enum.GetValues(typeof(CurrencyTypeEnum)).Cast<CurrencyTypeEnum>().ToArray(), currencyTypeEnum);
                currencyTypeId += 1;

                employmentDetails[i] = new JobOfferEmploymentDetails()
                {
                    EmploymentTypeId = employmentTypeId,
                    SalaryRange = new JobOfferSalariesRange()
                    {
                        From = salaryFrom,
                        To = salaryTo,
                        SalaryCurrencyId = currencyTypeId
                    }
                };
            }

            to.JobOfferEmploymentDetails = employmentDetails;
        }

        private void MapTechKeyWords(JobOfferUpdateViewModel from, JobOffer to)
        {
            var techKeyWords = new TechKeyWord[from.TechKeyWords.Length];

            for (int i = 0; i < techKeyWords.Length; i++)
            {
                string keyWord = from.TechKeyWords[i];

                techKeyWords[i] = new TechKeyWord()
                {
                    Name = keyWord
                };
            }

            to.TechKeyWords = techKeyWords;
        }
    }
}

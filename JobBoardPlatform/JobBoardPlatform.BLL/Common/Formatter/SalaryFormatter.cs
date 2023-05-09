using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Common.Formatter
{
    public class SalaryFormatter : ITextFormatter<JobOffer>
    {
        public string GetString(JobOffer offer)
        {
            string salaryDetails = "Undisclosed Salary";

            if (offer.JobOfferEmploymentDetails == null || offer.JobOfferEmploymentDetails.Count == 0)
            {
                return salaryDetails;
            }

            var details = offer.JobOfferEmploymentDetails.Where(x => x.SalaryRange != null);
            
            if (details == null || details.Count() == 0)
            {
                return salaryDetails;
            }

            var firstDetails = details.OrderBy(x => x.SalaryRange!.To).First();
            var salary = firstDetails.SalaryRange!;
            salaryDetails = $"{salary.From} - {salary.To} {salary.SalaryCurrency.Type}";

            return salaryDetails;
        }
    }
}

using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Common.Formatter
{
    public class SalaryFormatter : ITextFormatter<JobOfferSalariesRange?>
    {
        public string GetString(JobOfferSalariesRange? range)
        {
            string salaryDetails = "Undisclosed Salary";

            if (range == null)
            {
                return salaryDetails;
            }

            return $"{range.From} - {range.To} {range.SalaryCurrency.Type}";
        }
    }
}

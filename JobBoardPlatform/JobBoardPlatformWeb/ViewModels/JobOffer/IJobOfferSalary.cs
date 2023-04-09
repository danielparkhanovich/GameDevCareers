using JobBoardPlatform.DAL.Models.Enums;
using Microsoft.Build.Framework;

namespace JobBoardPlatform.PL.ViewModels.JobOffer
{
    public interface IJobOfferSalary
    {
        public string[] EmploymentTypes { get; set; }

        public int[]? SalaryFromRange { get; set; }

        public int[]? SalaryToRange { get; set; }

        public string[]? SalaryCurrency { get; set; }
    }
}

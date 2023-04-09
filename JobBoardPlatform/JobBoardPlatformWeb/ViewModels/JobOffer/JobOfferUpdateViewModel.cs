using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.PL.ViewModels.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.JobOffer
{
    public class JobOfferUpdateViewModel : IJobOfferSalary
    {
        [Required]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string WorkLocationType { get; set; } = string.Empty;

        [Required]
        [UniqueElements(ErrorMessage = "Employment types must be unique.")]
        public string[] EmploymentTypes { get; set; }

        public int[]? SalaryFromRange { get; set; }

        public int[]? SalaryToRange { get; set; }

        public string[]? SalaryCurrency { get; set; }

        public string[]? TechStack { get; set; }

        [Required]
        public string JobDescription { get; set; } = string.Empty;
    }
}

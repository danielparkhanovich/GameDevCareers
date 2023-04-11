using JobBoardPlatform.PL.ViewModels.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company
{
    public class JobOfferUpdateViewModel : IJobOfferSalary, IJobField
    {
        [Required]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public int WorkLocationType { get; set; }

        [Required]
        public string JobDescription { get; set; } = string.Empty;

        [Required]
        public int MainTechnology { get; set; }

        [Required]
        [UniqueElements(ErrorMessage = "Employment types must be unique.")]
        public string[] EmploymentTypes { get; set; }

        [IntElements(ErrorMessage = "From must be a number")]
        public int[]? SalaryFromRange { get; set; }

        [IntElements(ErrorMessage = "To must be a number")]
        public int[]? SalaryToRange { get; set; }

        public string[]? SalaryCurrency { get; set; }

        public string[]? TechKeyWords { get; set; }

        public string? Address { get; set; }
    }
}

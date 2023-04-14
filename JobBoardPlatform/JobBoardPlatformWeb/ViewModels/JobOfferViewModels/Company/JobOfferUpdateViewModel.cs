using JobBoardPlatform.PL.ViewModels.Attributes;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company
{
    public class JobOfferUpdateViewModel : IJobOfferSalary, IMainTechnology, ITechKeywords
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
        public int ContactType { get; set; }

        public string? ContactAddress { get; set; }

        [Required]
        public int MainTechnologyType { get; set; }

        [Required]
        [UniqueElements(ErrorMessage = "Employment types must be unique.")]
        public int[] EmploymentTypes { get; set; }

        [IntElements(ErrorMessage = "From must be a number")]
        public int[]? SalaryFromRange { get; set; }

        [IntElements(ErrorMessage = "To must be a number")]
        public int[]? SalaryToRange { get; set; }

        public int[]? SalaryCurrencyType { get; set; }

        public string[]? TechKeywords { get; set; }

        public string? Address { get; set; }
    }
}

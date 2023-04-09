using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.DAL.Models.EnumTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models
{
    [Table("JobOffers")]
    public class JobOffer : IEntity
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string WorkLocationType { get; set; } = string.Empty;
        public ICollection<EmploymentType> EmploymentTypes { get; set; }
        public ICollection<int> SalaryFromRange { get; set; }
        public ICollection<int> SalaryToRange { get; set; }
        public ICollection<CurrencyTypeEnum> SalaryCurrency { get; set; }

    }
}

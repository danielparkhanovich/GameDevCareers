using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.EnumTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models
{
    [Table("JobOfferEmploymentDetails")]
    public class JobOfferEmploymentDetails : IEntity
    {
        public int Id { get; set; }
        public ICollection<EmploymentType> EmploymentTypes { get; set; }
        public ICollection<int> SalaryFromRange { get; set; }
        public ICollection<int> SalaryToRange { get; set; }
        public ICollection<CurrencyType> SalaryCurrency { get; set; }
    }
}

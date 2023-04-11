using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.EnumTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Company
{
    [Table("JobOfferEmploymentDetails")]
    public class JobOfferEmploymentDetails : IEntity
    {
        public int Id { get; set; }

        [ForeignKey("EmploymentType")]
        public int EmploymentTypeId { get; set; }
        public EmploymentType EmploymentType { get; set; }

        [ForeignKey("SalaryRange")]
        public int SalaryRangeId { get; set; }
        public JobOfferSalariesRange? SalaryRange { get; set; }
    }
}

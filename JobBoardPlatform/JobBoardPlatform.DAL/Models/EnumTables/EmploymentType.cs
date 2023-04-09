using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.EnumTables
{
    [Table("EmploymentTypes")]
    public class EmploymentType : IEntity
    {
        public int Id { get; set; }
        public EmploymentTypeEnum Type { get; set; }
    }
}

using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.EnumTables
{
    [Table("EmploymentTypes")]
    public class EmploymentType : IEntity
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}

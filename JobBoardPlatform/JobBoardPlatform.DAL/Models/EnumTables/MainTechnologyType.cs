using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.EnumTables
{
    [Table("MainTechnologyTypes")]
    public class MainTechnologyType : IEnumEntity
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}

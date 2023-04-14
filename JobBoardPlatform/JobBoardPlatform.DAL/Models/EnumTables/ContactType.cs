using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.EnumTables
{
    [Table("ContactTypes")]
    public class ContactType : IEnumEntity
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}

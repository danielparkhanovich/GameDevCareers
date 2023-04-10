using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.EnumTables
{
    [Table("CurrencyTypes")]
    public class CurrencyType : IEntity
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}

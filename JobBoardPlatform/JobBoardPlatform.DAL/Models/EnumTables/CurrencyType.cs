using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.EnumTables
{
    [Table("CurrencyTypes")]
    public class CurrencyType : IEntity
    {
        public int Id { get; set; }
        public CurrencyTypeEnum Type { get; set; }
    }
}

using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.EnumTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Company
{
    [Table("JobOfferSalariesRange")]
    public class JobOfferSalariesRange : IEntity
    {
        public int Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        [ForeignKey("SalaryCurrency")]
        public int SalaryCurrencyId { get; set; }
        public CurrencyType SalaryCurrency { get; set; }
    }
}

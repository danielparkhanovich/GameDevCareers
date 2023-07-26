using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.EnumTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Company
{
    public class JobOfferContactDetails : IEntity
    {
        public int Id { get; set; }

        [ForeignKey("ContactType")]
        public int ContactTypeId { get; set; }
        public ContactType ContactType { get; set; }

        public string? ContactAddress { get; set; }
    }
}

using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models
{
    public class JobPost : IEntity
    {
        public int Id { get; set; }
        [ForeignKey("Company")]
        public int CompanyRefId { get; set; }
        public CompanyCredentials Company { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}

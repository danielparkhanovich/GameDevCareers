using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Models.EnumTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Company
{
    [Table("OfferApplications")]
    public class OfferApplication : IEntity
    {
        public int Id { get; set; }

        [ForeignKey("JobOffer")]
        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }

        [ForeignKey("ApplicationFlagType")]
        public int ApplicationFlagTypeId { get; set; }
        public ApplicationFlagType ApplicationFlagType { get; set; }

        [ForeignKey("EmployeeProfile")]
        public int? EmployeeProfileId { get; set; }
        public EmployeeProfile? EmployeeProfile { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

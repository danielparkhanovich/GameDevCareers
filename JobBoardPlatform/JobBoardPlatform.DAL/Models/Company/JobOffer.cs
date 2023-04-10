using JobBoardPlatform.DAL.Models.Company.Company;
using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Company
{
    [Table("JobOffers")]
    public class JobOffer : IEntity
    {
        public int Id { get; set; }

        [ForeignKey("CompanyProfile")]
        public int CompanyProfileId { get; set; }
        public CompanyProfile CompanyProfile { get; set; }

        public string JobTitle { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string WorkLocationType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool IsPaid { get; set; }

        public ICollection<JobOfferEmploymentDetails> JobOfferEmploymentDetails { get; set; }
        public ICollection<TechKeyWord> TechKeyWords { get; set; }
    }
}

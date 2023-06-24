using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.EnumTables;
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

        [ForeignKey("WorkLocation")]
        public int WorkLocationId { get; set; }
        public WorkLocationType WorkLocation { get; set; }

        [ForeignKey("MainTechnologyType")]
        public int MainTechnologyTypeId { get; set; }
        public MainTechnologyType MainTechnologyType { get; set; }

        [ForeignKey("ContactDetails")]
        public int ContactDetailsId { get; set; }
        public ContactDetails ContactDetails { get; set; }

        public string JobTitle { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime PublishedAt { get; set; }

        public bool IsPaid { get; set; }
        public bool IsPublished { get; set; }
        public bool IsShelved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSuspended { get; set; }

        public int NumberOfApplications { get; set; }
        public int NumberOfViews { get; set; }

        public virtual ICollection<JobOfferEmploymentDetails> JobOfferEmploymentDetails { get; set; }
        public virtual ICollection<TechKeyword> TechKeywords { get; set; }
        public virtual ICollection<OfferApplication> OfferApplications { get; set; }
    }
}

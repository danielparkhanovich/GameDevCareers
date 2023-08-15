using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.EnumTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Company
{
    [Table("JobOffers")]
    public class JobOffer : IEntity
    {
        public int Id { get; set; }

        [ForeignKey("Plan")]
        public int PlanId { get; set; }
        public JobOfferPlan Plan { get; set; }

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
        public JobOfferContactDetails ContactDetails { get; set; }

        public string JobTitle { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime RefreshedOnPageAt { get; set; }

        public string InformationClause { get; set; } = string.Empty;
        public string? ProcessingDataInFutureClause { get; set; }
        public string? CustomConsentClauseTitle { get; set; }
        public string? CustomConsentClause { get; set; }

        public bool IsPaid { get; set; }
        public bool IsPublished { get; set; }
        public bool IsShelved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSuspended { get; set; }

        public int NumberOfApplications { get; set; }
        public int NumberOfViews { get; set; }

        public virtual ICollection<JobOfferEmploymentDetails> EmploymentDetails { get; set; }
        public virtual ICollection<JobOfferTechKeyword> TechKeywords { get; set; }
        public virtual ICollection<JobOfferApplication> OfferApplications { get; set; }
    }
}

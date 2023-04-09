using JobBoardPlatform.DAL.Models.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models
{
    [Table("JobOffers")]
    public class JobOffer : IEntity
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string WorkLocationType { get; set; } = string.Empty;

        [ForeignKey("JobOfferEmploymentDetails")]
        public int JobOfferEmploymentDetailsId { get; set; }
        public JobOfferEmploymentDetails JobOfferEmploymentDetails { get; set; }
    }
}

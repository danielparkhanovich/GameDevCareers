using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.EnumTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.DAL.Models.Company
{
    [Table("JobOfferPlans")]
    public class JobOfferPlan : IEntity
    {
        public int Id { get; set; }
        public int PriceInPLN { get; set; }
        public int PublicationDaysCount { get; set; }
        public int EmploymentLocationsCount { get; set; }
        public int OfferRefreshesCount { get; set; }
        public bool IsAbleToRedirectApplications { get; set; }
        public int FreeSlotsCount { get; set; }

        [ForeignKey("Name")]
        public int NameId { get; set; }
        public JobOfferPlanType Name { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public JobOfferCategoryType Category { get; set; }
    }
}

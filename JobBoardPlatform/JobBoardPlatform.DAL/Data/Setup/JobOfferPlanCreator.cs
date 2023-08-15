using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Setup
{
    internal class JobOfferPlanCreator
    {
        private readonly ModelBuilder modelBuilder;


        public JobOfferPlanCreator(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void CreateRecords()
        {
            var getPlan = new Dictionary<JobOfferPlanEnum, Func<JobOfferPlan>>()
            {
                { JobOfferPlanEnum.COMMISSION, CreateCommissionPlan },
                { JobOfferPlanEnum.INDIE, CreateIndiePlan },
                { JobOfferPlanEnum.AAA, CreateAAAPlan },
            };

            var enumValues = Enum.GetValues(typeof(JobOfferPlanEnum))
                .Cast<JobOfferPlanEnum>()
                .ToArray();

            var enumData = new JobOfferPlan[enumValues.Length];

            for (int i = 0; i < enumValues.Length; i++)
            {
                var enumValue = enumValues[i];
                var getPlanFunc = getPlan[enumValue];
                var plan = getPlanFunc();
                enumData[i] = plan;
            }

            modelBuilder.Entity<JobOfferPlan>().HasData(enumData);
        }

        private JobOfferPlan CreateCommissionPlan()
        {
            return new JobOfferPlan()
            {
                Id = 1,
                NameId = (int)JobOfferPlanEnum.COMMISSION + 1,
                PriceInPLN = 25,
                PublicationDaysCount = 30,
                EmploymentLocationsCount = 1,
                IsAbleToRedirectApplications = true,
                OfferRefreshesCount = 1,
                FreeSlotsCount = 0,
                CategoryId = (int)JobOfferCategoryEnum.Commissions + 1,
            };
        }

        private JobOfferPlan CreateIndiePlan() 
        {
            return new JobOfferPlan()
            {
                Id = 2,
                NameId = (int)JobOfferPlanEnum.INDIE + 1,
                PriceInPLN = 50,
                PublicationDaysCount = 30,
                EmploymentLocationsCount = 3,
                IsAbleToRedirectApplications = false,
                OfferRefreshesCount = 3,
                FreeSlotsCount = 50,
                CategoryId = (int)JobOfferCategoryEnum.Employment + 1,
            };
        }

        private JobOfferPlan CreateAAAPlan()
        {
            return new JobOfferPlan()
            {
                Id = 3,
                NameId = (int)JobOfferPlanEnum.AAA + 1,
                PriceInPLN = 125,
                PublicationDaysCount = 45,
                EmploymentLocationsCount = 10,
                IsAbleToRedirectApplications = true,
                OfferRefreshesCount = 7,
                FreeSlotsCount = 0,
                CategoryId = (int)JobOfferCategoryEnum.Employment + 1,
            };
        }
    }
}

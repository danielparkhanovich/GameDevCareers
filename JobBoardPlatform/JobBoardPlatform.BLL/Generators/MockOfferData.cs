using JobBoardPlatform.BLL.DTOs;

namespace JobBoardPlatform.BLL.Generators
{
    internal class MockOfferData : OfferData
    {
        public int OfferId { get; set; }
        public int PlanId { get; set; }
        public string JobTitle { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int WorkLocationType { get; set; }
        public string JobDescription { get; set; }
        public int ApplicationsContactType { get; set; }
        public string? ApplicationsContactEmail { get; set; }
        public int MainTechnologyType { get; set; }
        public EmploymentType[] EmploymentTypes { get; set; }
        public string[]? TechKeywords { get; set; }
        public string? Street { get; set; }
        public string? ApplicationsContactExternalFormUrl { get; set; }
        public string InformationClause { get; set; }
        public bool IsDisplayConsentForFutureRecruitment { get; set; }
        public string? ConsentForFutureRecruitmentContent { get; set; }
        public bool IsDisplayCustomConsent { get; set; }
        public string? CustomConsentTitle { get; set; }
        public string? CustomConsentContent { get; set; }
    }
}

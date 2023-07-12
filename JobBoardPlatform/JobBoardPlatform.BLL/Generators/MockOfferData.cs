using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.BLL.Generators
{
    internal class MockOfferData : INewOfferData
    {
        public int OfferId { get; set; }
        public string JobTitle { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int WorkLocationType { get; set; }
        public string JobDescription { get; set; }
        public int ApplicationsContactType { get; set; }
        public string? ApplicationsContactEmail { get; set; }
        public int MainTechnologyType { get; set; }
        public int[] EmploymentTypes { get; set; }
        public int[]? SalaryFromRange { get; set; }
        public int[]? SalaryToRange { get; set; }
        public int[]? SalaryCurrencyType { get; set; }
        public string[]? TechKeywords { get; set; }
        public string? Street { get; set; }
        public string? ApplicationsContactExternalFormUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string InformationClause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDisplayConsentForFutureRecruitment { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? ConsentForFutureRecruitmentContent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDisplayCustomConsent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? CustomConsentTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? CustomConsentContent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}

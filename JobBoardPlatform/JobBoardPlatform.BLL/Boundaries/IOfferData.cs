namespace JobBoardPlatform.BLL.Boundaries
{
    public interface IOfferData
    {
        int OfferId { get; set; }
        string JobTitle { get; set; }
        string Country { get; set; } // TODO: Remove
        string City { get; set; }
        string? Street { get; set; }
        int WorkLocationType { get; set; }
        EmploymentType[] EmploymentTypes { get; set; }
        int MainTechnologyType { get; set; } // change to enum
        string[]? TechKeywords { get; set; }
        string JobDescription { get; set; }
        int ApplicationsContactType { get; set; } // change to enum
        string? ApplicationsContactEmail { get; set; }
        string? ApplicationsContactExternalFormUrl { get; set; }
        string InformationClause { get; set; }
        bool IsDisplayConsentForFutureRecruitment { get; set; }
        string? ConsentForFutureRecruitmentContent { get; set; }
        bool IsDisplayCustomConsent { get; set; }
        string? CustomConsentTitle { get; set; }
        string? CustomConsentContent { get; set; }
    }
}

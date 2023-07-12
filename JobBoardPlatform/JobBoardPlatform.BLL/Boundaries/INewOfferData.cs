namespace JobBoardPlatform.BLL.Boundaries
{
    public interface INewOfferData
    {
        int OfferId { get; set; }

        string JobTitle { get; set; }

        string Country { get; set; } // TODO: Remove

        string City { get; set; }

        string? Street { get; set; }

        int WorkLocationType { get; set; }

        int[] EmploymentTypes { get; set; }

        int[]? SalaryFromRange { get; set; }

        int[]? SalaryToRange { get; set; }

        int[]? SalaryCurrencyType { get; set; } // change to enum

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

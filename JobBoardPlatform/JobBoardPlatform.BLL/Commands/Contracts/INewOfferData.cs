namespace JobBoardPlatform.BLL.Models.Contracts
{
    public interface INewOfferData
    {
        string JobTitle { get; set; }

        string Country { get; set; }

        string City { get; set; }

        int WorkLocationType { get; set; }

        string JobDescription { get; set; }

        int ContactType { get; set; }

        string? ContactAddress { get; set; }

        int MainTechnologyType { get; set; }

        int[] EmploymentTypes { get; set; }

        int[]? SalaryFromRange { get; set; }

        int[]? SalaryToRange { get; set; }

        int[]? SalaryCurrencyType { get; set; }

        string[]? TechKeywords { get; set; }

        string? Address { get; set; }
    }
}

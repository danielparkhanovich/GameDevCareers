using JobBoardPlatform.BLL.Models.Contracts;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.DataStructures
{
    internal class OfferDataMock : INewOfferData
    {
        public int OfferId { get; set; }
        public string JobTitle { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int WorkLocationType { get; set; }
        public string JobDescription { get; set; }
        public int ContactType { get; set; }
        public string? ContactAddress { get; set; }
        public int MainTechnologyType { get; set; }
        public int[] EmploymentTypes { get; set; }
        public int[]? SalaryFromRange { get; set; }
        public int[]? SalaryToRange { get; set; }
        public int[]? SalaryCurrencyType { get; set; }
        public string[]? TechKeywords { get; set; }
        public string? Address { get; set; }
    }
}

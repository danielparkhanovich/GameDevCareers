using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.BLL.Generators
{
    internal class MockEmploymentType : EmploymentType
    {
        public int EmploymentType { get; set; }
        public int? SalaryFromRange { get; set; }
        public int? SalaryToRange { get; set; }
        public int? SalaryCurrencyType { get; set; }
    }
}

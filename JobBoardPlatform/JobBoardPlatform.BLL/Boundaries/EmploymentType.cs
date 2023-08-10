namespace JobBoardPlatform.BLL.Boundaries
{
    public class EmploymentType
    {
        public int TypeId { get; set; }
        public int? SalaryFromRange { get; set; }
        public int? SalaryToRange { get; set; }
        public int? SalaryCurrencyType { get; set; }
    }
}

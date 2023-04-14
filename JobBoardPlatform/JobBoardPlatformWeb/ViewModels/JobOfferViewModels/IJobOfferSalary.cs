namespace JobBoardPlatform.PL.ViewModels.JobOfferViewModels
{
    public interface IJobOfferSalary
    {
        public int[] EmploymentTypes { get; set; }

        public int[]? SalaryFromRange { get; set; }

        public int[]? SalaryToRange { get; set; }

        public int[]? SalaryCurrencyType { get; set; }
    }
}

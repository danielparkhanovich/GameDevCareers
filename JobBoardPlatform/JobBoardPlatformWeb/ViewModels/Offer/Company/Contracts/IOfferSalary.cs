namespace JobBoardPlatform.PL.ViewModels.Offer.Company.Contracts
{
    public interface IOfferSalary
    {
        public int[] EmploymentTypes { get; set; }

        public int[]? SalaryFromRange { get; set; }

        public int[]? SalaryToRange { get; set; }

        public int[]? SalaryCurrencyType { get; set; }
    }
}

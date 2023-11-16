using JobBoardPlatform.BLL.DTOs;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company.Contracts
{
    public interface IOfferSalary
    {
        public EmploymentType[] EmploymentTypes { get; set; }
    }
}

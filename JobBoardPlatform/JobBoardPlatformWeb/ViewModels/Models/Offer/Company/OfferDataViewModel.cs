using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company
{
    public class OfferDataViewModel : OfferData, IMainTechnology, ITechKeywords, IOfferSalary
    {
        public OfferDataViewModel()
        {
            this.MainTechnologyType = 1;
            this.EmploymentTypes = new EmploymentType[1] { new EmploymentType() };
            this.InformationClause =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                "Pellentesque ac massa non felis tincidunt ornare. " +
                "Aenean convallis commodo nibh, sit amet iaculis turpis molestie ac.";
        }
    }
}

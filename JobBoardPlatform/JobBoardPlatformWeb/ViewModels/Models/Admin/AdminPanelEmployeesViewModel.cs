using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.ViewModels.Models.Templates;

namespace JobBoardPlatform.PL.ViewModels.Models.Admin
{
    public class AdminPanelEmployeesViewModel : IAdminPanelViewModel<EmployeeIdentity>
    {
        public CardsContainerViewModel CardsContainer { get; set; }
        public List<EmployeeIdentity> AllRecords { get; set; }
        public int CountToGenerate { get; set; }
    }
}

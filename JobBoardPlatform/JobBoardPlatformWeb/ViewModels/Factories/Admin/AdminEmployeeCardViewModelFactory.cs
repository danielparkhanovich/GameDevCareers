using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.Controllers.Utils;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Admin;

namespace JobBoardPlatform.PL.ViewModels.Factories.Admin
{
    public class AdminEmployeeCardViewModelFactory : IContainerCardFactory<EmployeeIdentity>
    {
        public IContainerCard CreateCard(EmployeeIdentity employee)
        {
            var card = new AdminEmployeeCardViewModel();

            card.Id = employee.Id;
            card.Email = employee.Email;
            card.Name = employee.Profile.Name;
            card.Surname = employee.Profile.Surname;
            card.Country = employee.Profile.Country;
            card.City = employee.Profile.City;
            card.ProfileImageUrl = GetProfileImageUri(employee);
            card.AttachedResumeUrl = employee.Profile.ResumeUrl;
            card.YearsOfExperience = employee.Profile.YearsOfExperience;
            card.LinkedInUrl = employee.Profile.LinkedInUrl;

            return card;
        }

        private string GetProfileImageUri(EmployeeIdentity employee)
        {
            return StaticFilesUtils.GetEmployeeDefaultAvatarUriIfEmpty(employee.Profile.ProfileImageUrl);
        }
    }
}

using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.ViewModels.Profile.Employee;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Utilities.Mappers.Profile
{
    internal class EmployeeViewModelToProfileMapper : IMapper<EmployeeProfileViewModel, EmployeeProfile>
    {
        public void Map(EmployeeProfileViewModel from, EmployeeProfile to)
        {
            var fromUpdate = from.Update;

            if (!string.IsNullOrEmpty(fromUpdate.Name))
            {
                to.Name = fromUpdate.Name;
            }
            if (fromUpdate.AttachedResume != null && !string.IsNullOrEmpty(fromUpdate.AttachedResume.ResumeUrl))
            {
                to.ResumeUrl = fromUpdate.AttachedResume.ResumeUrl;
            }
            if (!string.IsNullOrEmpty(fromUpdate.ProfileImageUrl))
            {
                to.ProfileImageUrl = fromUpdate.ProfileImageUrl;
            }

            to.Surname = fromUpdate.Surname;
            to.City = fromUpdate.City;
            to.Country = fromUpdate.Country;
            to.Description = fromUpdate.Description;
            to.YearsOfExperience = fromUpdate.YearsOfExperience;
            to.LinkedInUrl = fromUpdate.LinkedInUrl;
        }
    }
}

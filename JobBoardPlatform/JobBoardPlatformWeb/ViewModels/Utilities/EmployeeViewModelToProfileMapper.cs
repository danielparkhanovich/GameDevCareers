using JobBoardPlatform.BLL.Services.Utilities.Contracts;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.PL.ViewModels.Profile;

namespace JobBoardPlatform.BLL.Services.Utilities
{
    internal class EmployeeViewModelToProfileMapper : IMapper<EmployeeProfileViewModel, EmployeeProfile>
    {
        public void Map(EmployeeProfileViewModel from, EmployeeProfile to)
        {
            if (!string.IsNullOrEmpty(from.Name))
            {
                to.Name = from.Name;
            }
            if (!string.IsNullOrEmpty(from.Surname))
            {
                to.Surname = from.Surname;
            }
            if (!string.IsNullOrEmpty(from.City))
            {
                to.City = from.City;
            }
            if (!string.IsNullOrEmpty(from.Country))
            {
                to.Country = from.Country;
            }
            if (!string.IsNullOrEmpty(from.Description))
            {
                to.Description = from.Description;
            }
            if (!string.IsNullOrEmpty(from.ResumeUrl))
            {
                to.ResumeUrl = from.ResumeUrl;
            }
            if (!string.IsNullOrEmpty(from.PhotoUrl))
            {
                to.ProfileImageUrl = from.PhotoUrl;
            }
        }
    }
}

using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Employee;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Profile
{
    public class EmployeeProfileViewModelFactory : IViewModelFactory<EmployeeProfile, EmployeeProfileViewModel>
    {
        private readonly string resumeName;
        private readonly string resumeSize;


        public EmployeeProfileViewModelFactory(string resumeName, string resumeSize)
        {
            this.resumeName = resumeName;
            this.resumeSize = resumeSize;
        }

        public EmployeeProfileViewModel CreateViewModel(EmployeeProfile profile)
        {
            var viewModel = new EmployeeProfileViewModel()
            {
                Name = profile.Name,
                Surname = profile.Surname,
                City = profile.City,
                Country = profile.Country,
                Description = profile.Description,
                ProfileImageUrl = profile.ProfileImageUrl,
                ResumeUrl = profile.ResumeUrl,
                FileName = resumeName,
                FileSize = resumeSize,
                YearsOfExperience = profile.YearsOfExperience,
                LinkedInUrl = profile.LinkedInUrl
            };

            return viewModel;
        }
    }
}

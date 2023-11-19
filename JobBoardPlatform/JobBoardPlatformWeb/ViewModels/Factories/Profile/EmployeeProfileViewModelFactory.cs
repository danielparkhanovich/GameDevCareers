using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.ViewModels.Factories.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Common;
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

        public EmployeeProfileViewModel Create(EmployeeProfile profile)
        {
            var viewModel = new EmployeeProfileViewModel()
            {
                Name = profile.Name,
                Surname = profile.Surname,
                City = profile.City,
                Country = profile.Country,
                Description = profile.Description,
                ProfileImage = GetProfileImage(profile.ProfileImageUrl),
                ResumeUrl = profile.ResumeUrl,
                FileName = resumeName,
                FileSize = resumeSize,
                YearsOfExperience = profile.YearsOfExperience,
                LinkedInUrl = profile.LinkedInUrl,
                AttachedResume = GetAttachedResume(profile.ResumeUrl),
            };

            return viewModel;
        }

        private ProfileImage GetProfileImage(string? imageUrl)
        {
            return new ProfileImageViewModel()
            {
                ImageUrl = imageUrl
            };
        }

        private EmployeeAttachedResumeViewModel GetAttachedResume(string? resumeUrl)
        {
            return new EmployeeAttachedResumeViewModel()
            {
                FileName = resumeName,
                FileSize = resumeSize,
                ResumeUrl = resumeUrl
            };
        }
    }
}

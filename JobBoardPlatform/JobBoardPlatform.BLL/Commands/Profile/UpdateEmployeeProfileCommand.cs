using JobBoardPlatform.BLL.Models.Contracts;
using JobBoardPlatform.BLL.Services.Session;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public class UpdateEmployeeProfileCommand : UpdateProfileCommandBase<EmployeeIdentity, EmployeeProfile, IEmployeeProfileData>
    {
        private readonly IBlobStorage userProfileImagesStorage;
        private readonly IBlobStorage userResumesStorage;


        public UpdateEmployeeProfileCommand(int profileId,
            IEmployeeProfileData profileData, 
            IRepository<EmployeeProfile> repository,
            HttpContext httpContext,
            IUserSessionService<EmployeeIdentity, EmployeeProfile> userSession,
            IBlobStorage userProfileImagesStorage,
            IBlobStorage userResumesStorage) 
            : base(profileId, profileData, repository, httpContext, userSession)
        {
            this.userProfileImagesStorage = userProfileImagesStorage;
            this.userResumesStorage = userResumesStorage;
        }

        protected override async Task UploadFiles(IEmployeeProfileData from, EmployeeProfile to)
        {
            if (from.ProfileImage != null)
            {
                var imageUrl = await userProfileImagesStorage.UpdateAsync(to.ProfileImageUrl, from.ProfileImage);
                to.ProfileImageUrl = imageUrl;
            }
            if (from.File != null)
            {
                var resumeUrl = await userResumesStorage.UpdateAsync(to.ResumeUrl, from.File);
                to.ResumeUrl = resumeUrl;
            }
        }

        protected override void MapDataToModel(IEmployeeProfileData from, EmployeeProfile to)
        {
            if (!string.IsNullOrEmpty(from.Name))
            {
                to.Name = from.Name;
            }
            if (!string.IsNullOrEmpty(from.ResumeUrl))
            {
                to.ResumeUrl = from.ResumeUrl;
            }
            if (!string.IsNullOrEmpty(from.ProfileImageUrl))
            {
                to.ProfileImageUrl = from.ProfileImageUrl;
            }

            to.Surname = from.Surname;
            to.City = from.City;
            to.Country = from.Country;
            to.Description = from.Description;
            to.YearsOfExperience = from.YearsOfExperience;
            to.LinkedInUrl = from.LinkedInUrl;
        }
    }
}

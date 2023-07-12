using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public class UpdateEmployeeProfileCommand : UpdateProfileCommandBase<EmployeeIdentity, EmployeeProfile, IEmployeeProfileData>
    {
        private readonly IUserProfileImagesStorage imageStorage;
        private readonly IProfileResumeBlobStorage resumeStorage;


        public UpdateEmployeeProfileCommand(int profileId,
            IEmployeeProfileData profileData, 
            IRepository<EmployeeProfile> repository,
            IUserProfileImagesStorage imageStorage,
            IProfileResumeBlobStorage resumeStorage) 
            : base(profileId, profileData, repository)
        {
            this.imageStorage = imageStorage;
            this.resumeStorage = resumeStorage;
        }

        protected override async Task UploadFiles(IEmployeeProfileData from, EmployeeProfile to)
        {
            if (from.ProfileImage != null && from.ProfileImage.File != null)
            {
                var imageUrl = await imageStorage.ChangeImageAsync(to.ProfileImageUrl, from.ProfileImage.File);
                to.ProfileImageUrl = imageUrl;
            }
            if (from.File != null)
            {
                var resumeUrl = await resumeStorage.ChangeResumeAsync(to.ResumeUrl, from.File);
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
            if (!string.IsNullOrEmpty(from.ProfileImage.ImageUrl))
            {
                to.ProfileImageUrl = from.ProfileImage.ImageUrl;
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

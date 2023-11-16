using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Commands.Profile;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Blob.Metadata;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.IntegrationTests.Common.Mocks.DataStructures;
using JobBoardPlatform.IntegrationTests.Common.TestFiles;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Common.Utils
{
    internal class EmployeeIntegrationTestsUtils
    {
        private readonly UserManager<EmployeeIdentity> userManager;
        private readonly IPasswordHasher passwordHasher;
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IUserProfileImagesStorage imageStorage;
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsManager applicationsManager;
        private readonly IEmailContent<JobOfferApplication> applicationEmailRenderer;


        public EmployeeIntegrationTestsUtils(IServiceProvider serviceProvider)
        {
            userManager = serviceProvider.GetService<UserManager<EmployeeIdentity>>()!;
            passwordHasher = serviceProvider.GetService<IPasswordHasher>()!;
            profileRepository = serviceProvider.GetService<IRepository<EmployeeProfile>>()!;
            imageStorage = serviceProvider.GetService<IUserProfileImagesStorage>()!;
            profileResumeStorage = serviceProvider.GetService<IProfileResumeBlobStorage>()!;
            applicationsManager = serviceProvider.GetService<IApplicationsManager>()!;
            applicationEmailRenderer = serviceProvider.GetService<IEmailContent<JobOfferApplication>>()!;
        }

        public string GetUserExampleEmail(int userId = 1)
        {
            return $"employee{userId}@test.com";
        }

        public async Task AddExampleEmployeesAsync(int usersCount)
        {
            for (int i = 0; i < usersCount; i++) 
            {
                string email = GetUserExampleEmail(i + 1);
                await AddExampleEmployeeAsync(email);
            }
        }

        public Task AddExampleEmployeeAsync(string email)
        {
            return AddExampleEmployeeToRepositoryAsync(email, "1234567890");
        }

        public async Task AddExampleEmployeeToRepositoryAsync(string email, string password)
        {
            string passwordHash = passwordHasher.GetHash(password);
            var employee = new EmployeeIdentity()
            {
                Email = email,
                HashPassword = passwordHash,
                Profile = new EmployeeProfile()
            };
            await userManager.AddAsync(employee);
        }

        public async Task DeleteEmployee(string email)
        {
            var user = await userManager.GetWithEmailAsync(email);
            await userManager.DeleteAsync(user.Id);
        }

        public async Task TryDeleteEmployeeProfileResume(string email)
        {
            var user = await GetEmployeeByEmail(email);
            var deleteCommand = new DeleteEmployeeResumeCommand(
                user.ProfileId, profileRepository, profileResumeStorage);
            await deleteCommand.Execute();
        }

        public Task<EmployeeIdentity> GetEmployeeByEmail(string email)
        {
            return userManager.GetWithEmailAsync(email);
        }

        public async Task<EmployeeProfile> GetEmployeeProfileByEmail(string email)
        {
            var user = await userManager.GetWithEmailAsync(email);
            return await profileRepository.Get(user.ProfileId);
        }

        public async Task<EmployeeProfile> GetEmployeeProfileById(int profileId)
        {
            return await profileRepository.Get(profileId);
        }

        public Task<BlobDescription> GetResumeMetadataFromStorage(string url)
        {
            return profileResumeStorage.GetMetadataAsync(url);
        }

        public async Task<string> GetResumeUrl(string email)
        {
            var userProfile = await GetEmployeeProfileByEmail(email);
            return userProfile.ResumeUrl!;
        }

        public Task<bool> IsProfileImageInStorage(string url)
        {
            return imageStorage.IsImageExistsAsync(url);
        }

        public Task<bool> IsResumeInStorage(string url)
        {
            return profileResumeStorage.IsExistsAsync(url);
        }

        public Task ApplyToOffer(int usersCount, int offerId)
        {
            return ApplyToOffer(0, usersCount, offerId);
        }

        public async Task ApplyToOffer(int userIdFrom, int userIdTo, int offerId)
        {
            for (int i = userIdFrom; i < userIdTo; i++)
            {
                string email = GetUserExampleEmail(i + 1);
                await ApplyToOffer(email, offerId);
            }
        }

        public async Task ApplyToOffers(string email, JobOffer[] offers)
        {
            for (int i = 0; i < offers.Length; i++)
            {
                await ApplyToOffer(email, offers[i].Id);
            }
        }

        public async Task ApplyToOffer(string email, int offerId)
        {
            string? resumeUrl = null;
            int? profileId = null;
            var user = await GetEmployeeByEmail(email);
            if (user != null)
            {
                var profile = await profileRepository.Get(user.ProfileId);
                resumeUrl = profile.ResumeUrl;
                profileId = profile.Id;
            }

            var applicationForm = GetApplicationForm(email, offerId, resumeUrl);
            await applicationsManager.PostApplicationFormAsync(offerId, profileId, applicationForm, applicationEmailRenderer);
        }

        public async Task SetUsersResumeInProfile(int usersCount)
        {
            for (int i = 0; i < usersCount; i++)
            {
                string email = GetUserExampleEmail(i + 1);
                await SetUserResumeInProfile(email);
            }
        }

        public Task SetUserResumeInProfile(string email)
        {
            var resume = IntegrationTestFilesManager.GetExampleResumeFile();
            var profileData = new EmployeeProfileData() { File = resume };
            return UpdateEmployeeProfile(email, profileData);
        }

        public Task SetUserImageInProfile(string email)
        {
            var image = IntegrationTestFilesManager.GetEmployeeProfileImageFile();
            var profileData = new EmployeeProfileData() 
            { 
                ProfileImage = new ProfileImage() { File = image }
            };
            return UpdateEmployeeProfile(email, profileData);
        }

        private async Task UpdateEmployeeProfile(string email, EmployeeProfileData profileData)
        {
            var user = await GetEmployeeByEmail(email);
            await userManager.UpdateProfileAsync(user.Id, profileData);
        }

        private ApplicationForm GetApplicationForm(string email, int offerId, string? resumeUrl)
        {
            var attachedResume = new AttachedResume();
            if (!string.IsNullOrEmpty(resumeUrl))
            {
                attachedResume.ResumeUrl = resumeUrl;
            }
            else
            {
                attachedResume.File = IntegrationTestFilesManager.GetExampleResumeFile();
            }

            return new ApplicationForm()
            {
                FullName = "Example Example",
                AdditionalInformation = "SomeText",
                AttachedResume = attachedResume,
                Email = email,
                OfferId = offerId
            };
        }
    }
}

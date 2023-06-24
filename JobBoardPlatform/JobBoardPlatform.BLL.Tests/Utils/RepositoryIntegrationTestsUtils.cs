using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Commands.Contracts;
using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Commands.Profile;
using JobBoardPlatform.BLL.Models.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.IntegrationTests.Mocks;
using JobBoardPlatform.IntegrationTests.TestFiles;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Utils
{
    internal class RepositoryIntegrationTestsUtils
    {
        private readonly UserManager<EmployeeIdentity> userManager;
        private readonly IPasswordHasher passwordHasher;
        private readonly IRepository<EmployeeIdentity> repository;
        private readonly IRepository<EmployeeProfile> profileRepository;
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<OfferApplication> applicationRepositry;
        private readonly IUserProfileImagesStorage imageStorage;
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage applicationsResumeStorage;


        public RepositoryIntegrationTestsUtils(IServiceProvider serviceProvider)
        {
            this.userManager = serviceProvider.GetService<UserManager<EmployeeIdentity>>()!;
            this.passwordHasher = serviceProvider.GetService<IPasswordHasher>()!;
            this.repository = serviceProvider.GetService<IRepository<EmployeeIdentity>>()!;
            this.profileRepository = serviceProvider.GetService<IRepository<EmployeeProfile>>()!;
            this.offersRepository = serviceProvider.GetService<IRepository<JobOffer>>()!;
            this.applicationRepositry = serviceProvider.GetService<IRepository<OfferApplication>>()!;
            this.imageStorage = serviceProvider.GetService<IUserProfileImagesStorage>()!;
            this.profileResumeStorage = serviceProvider.GetService<IProfileResumeBlobStorage>()!;
            this.applicationsResumeStorage = serviceProvider.GetService<IApplicationsResumeBlobStorage>()!;
        }

        public Task AddExampleEmployeeToRepositoryAsync(string email)
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
            await userManager.AddNewUser(employee);
        }

        public async Task<DeleteEmployeeCommand> GetDeleteEmployeeCommandByEmail(string email)
        {
            var user = await userManager.GetUserByEmail(email);
            var deleteCommand = new DeleteEmployeeCommand(
                repository,
                profileRepository,
                imageStorage,
                profileResumeStorage,
                user.Id);
            return deleteCommand;
        }

        public Task<EmployeeIdentity> GetEmployeeByEmail(string email)
        {
            return userManager.GetUserByEmail(email);
        }

        public async Task<EmployeeProfile> GetEmployeeProfileByEmail(string email)
        {
            var user = await userManager.GetUserByEmail(email);
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

        public Task<bool> IsProfileImageInStorage(string url)
        {
            return imageStorage.IsImageExistsAsync(url);
        }

        public Task<bool> IsResumeInStorage(string url)
        {
            return profileResumeStorage.IsExistsAsync(url);
        }

        public async Task ApplyToOffer(string email, int offerId, string? resumeUrl = null)
        {
            var user = await GetEmployeeByEmail(email);
            var command = new PostApplicationFormCommand(
                applicationRepositry,
                offersRepository,
                applicationsResumeStorage,
                GetApplicationForm(email, offerId, resumeUrl),
                offerId,
                user.ProfileId);
            await command.Execute();
        }

        public Task SetUserResumeInProfile(string email)
        {
            var resume = IntegrationTestFilesManager.GetExampleResumeFile();
            var profileData = new EmployeeProfileDataMock() { File = resume };
            return UpdateEmployeeProfile(email, profileData);
        }

        public Task SetUserImageInProfile(string email)
        {
            var image = IntegrationTestFilesManager.GetEmployeeProfileImageFile();
            var profileData = new EmployeeProfileDataMock() { ProfileImage = image };
            return UpdateEmployeeProfile(email, profileData);
        }

        private async Task UpdateEmployeeProfile(string email, IEmployeeProfileData profileData)
        {
            var user = await GetEmployeeByEmail(email);
            var updateCommand = new UpdateEmployeeProfileCommand(
                user.Id,
                profileData,
                profileRepository,
                imageStorage,
                profileResumeStorage);
            await updateCommand.Execute();
        }

        private ApplicationFormMock GetApplicationForm(string email, int offerId, string? resumeUrl)
        {
            var attachedResume = new AttachedResumeMock();
            if (!string.IsNullOrEmpty(resumeUrl))
            {
                attachedResume.ResumeUrl = resumeUrl;
            }
            else
            {
                attachedResume.File = IntegrationTestFilesManager.GetExampleResumeFile();
            }

            return new ApplicationFormMock()
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

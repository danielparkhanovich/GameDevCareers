using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public class UpdateUserCommandFactory : IUpdateUserCommandFactory
    {
        private readonly IRepository<EmployeeProfile> employeeProfileRepository;
        private readonly IRepository<CompanyProfile> companyProfileRepository;
        private readonly IUserProfileImagesStorage imagesStorage;
        private readonly IProfileResumeBlobStorage resumesStorage;


        public UpdateUserCommandFactory(
            IRepository<EmployeeProfile> employeeProfileRepository,
            IRepository<CompanyProfile> companyProfileRepository,
            IUserProfileImagesStorage imagesStorage,
            IProfileResumeBlobStorage resumesStorage)
        {
            this.employeeProfileRepository = employeeProfileRepository;
            this.companyProfileRepository = companyProfileRepository;
            this.imagesStorage = imagesStorage;
            this.resumesStorage = resumesStorage;
        }

        public ICommand GetCommand(Type identityType, int profileId, ProfileData profileData)
        {
            if (identityType == typeof(EmployeeIdentity))
            {
                return GetUpdateEmployeeCommand(profileId, profileData);
            }
            else if (identityType == typeof(CompanyIdentity))
            {
                return GetUpdateCompanyCommand(profileId, profileData);
            }
            throw new Exception("Unknown user identity type");
        }

        private ICommand GetUpdateEmployeeCommand(int profileId, ProfileData profileData)
        {
            return new UpdateEmployeeProfileCommand(
                profileId, (EmployeeProfileData)profileData, employeeProfileRepository, imagesStorage, resumesStorage);
        }

        private ICommand GetUpdateCompanyCommand(int profileId, ProfileData profileData)
        {
            return new UpdateCompanyProfileCommand(
                profileId, (CompanyProfileData)profileData, companyProfileRepository, imagesStorage);
        }
    }
}

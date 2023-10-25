using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Mappers;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public class UpdateCompanyProfileCommand : UpdateProfileCommandBase<CompanyIdentity, CompanyProfile, CompanyProfileData>
    {
        private readonly IUserProfileImagesStorage imageStorage;


        public UpdateCompanyProfileCommand(int profileId, 
            CompanyProfileData profileData, 
            IRepository<CompanyProfile> repository,
            IUserProfileImagesStorage imageStorage) 
            : base(profileId, profileData, repository)
        {
            this.imageStorage = imageStorage;
        }

        protected override async Task UploadFiles(CompanyProfileData from, CompanyProfile to)
        {
            if (from.ProfileImage != null && from.ProfileImage.File != null)
            {
                var imageUrl = await imageStorage.ChangeImageAsync(to.ProfileImageUrl, from.ProfileImage.File);
                to.ProfileImageUrl = imageUrl;
            }
        }

        protected override void MapDataToModel(CompanyProfileData from, CompanyProfile to)
        {
            var mapper = new CompanyDataToCompanyProfileMapper();
            mapper.Map(from, to);
        }
    }
}

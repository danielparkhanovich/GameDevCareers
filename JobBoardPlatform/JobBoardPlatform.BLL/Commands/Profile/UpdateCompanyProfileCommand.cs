using JobBoardPlatform.BLL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.BLL.Commands.Profile
{
    public class UpdateCompanyProfileCommand : UpdateProfileCommandBase<CompanyIdentity, CompanyProfile, ICompanyProfileData>
    {
        private readonly IUserProfileImagesStorage imageStorage;


        public UpdateCompanyProfileCommand(int profileId, 
            ICompanyProfileData profileData, 
            IRepository<CompanyProfile> repository,
            IUserProfileImagesStorage imageStorage) 
            : base(profileId, profileData, repository)
        {
            this.imageStorage = imageStorage;
        }

        protected override async Task UploadFiles(ICompanyProfileData from, CompanyProfile to)
        {
            if (from.ProfileImage != null)
            {
                var imageUrl = await imageStorage.ChangeImageAsync(to.ProfileImageUrl, from.ProfileImage);
                to.ProfileImageUrl = imageUrl;
            }
        }

        protected override void MapDataToModel(ICompanyProfileData from, CompanyProfile to)
        {
            if (!string.IsNullOrEmpty(from.CompanyName))
            {
                to.CompanyName = from.CompanyName;
            }
            if (!string.IsNullOrEmpty(from.ProfileImageUrl))
            {
                to.ProfileImageUrl = from.ProfileImageUrl;
            }

            to.City = from.City;
            to.Country = from.Country;
            to.CompanyWebsiteUrl = from.CompanyWebsiteUrl;
        }
    }
}

using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Register;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob.Temporary;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class EmailCompanyPublishOfferAndRegistrationInteractor
    {
        private readonly IOfferManager offerManager;
        private readonly IUserProfileImagesTemporaryStorage temporaryStorage;
        private readonly DataTokensService<ICompanyProfileAndNewOfferData> dataTokensService;
        private readonly ConfirmationTokensService confirmationTokensService;
        private readonly EmailCompanyPublishOfferAndRegistrationService registrationService;


        public EmailCompanyPublishOfferAndRegistrationInteractor(
            IOfferManager offerManager,
            IUserProfileImagesTemporaryStorage temporaryStorage,
            DataTokensService<ICompanyProfileAndNewOfferData> dataTokensService,
            ConfirmationTokensService confirmationTokensService,
            EmailCompanyPublishOfferAndRegistrationService registrationService)
        {
            this.offerManager = offerManager;
            this.temporaryStorage = temporaryStorage;
            this.dataTokensService = dataTokensService;
            this.confirmationTokensService = confirmationTokensService;
            this.registrationService = registrationService;
        }

        /// <returns>Token Id to access data from cache</returns>
        public async Task<string> SavePostFormViewModelAsync(
            ICompanyProfileAndNewOfferData viewModel, string? previousTokenId = null)
        {
            if (!string.IsNullOrEmpty(previousTokenId))
            {
                await DeleteSavedDataAsync(previousTokenId);
            }

            var token = await SaveRegistrationDataAsync(viewModel);
            return token.Id;
        }

        public async Task<ICompanyProfileAndNewOfferData> GetPostFormViewModelAsync(string tokenId)
        {
            var token = await dataTokensService.TryGetTokenAsync(tokenId);
            return token.Value;
        }

        public async Task<JobOffer> GetAddedOfferAsync(int userProfileId)
        {
            var offerIds = await offerManager.GetAllIdsAsync(userProfileId);
            if (offerIds.Count == 0)
            {
                throw new Exception("Offer not added");
            }
            return await offerManager.GetAsync(offerIds.First());
        }

        public async Task<RedirectData> ProcessRegistrationAndRedirectAsync(
            CompanyRegisterViewModel registrationData, string tokenId)
        {
            await registrationService.TrySendConfirmationTokenAsync(
                registrationData.Email, registrationData.Password, tokenId);
            return RedirectData.NoRedirect;
        }

        /// <returns>Token Id to access data from cache</returns>
        public async Task<string> FinishRegistrationAsync(string tokenId, HttpContext httpContext)
        {
            await registrationService.TryRegisterByTokenAsync(tokenId, httpContext);
            return (await confirmationTokensService.TryGetTokenAsync(tokenId)).TokenToConfirmId;
        }

        public async Task DeletePreviousSavedDataAsync(ICompanyProfileAndNewOfferData viewModel, string tokenId)
        {
            await DeleteSavedViewModelAsync(tokenId);
            if (viewModel.CompanyProfileData.ProfileImage.File != null)
            {
                await DeleteSavedImageAsync(tokenId);
            }
        }

        private async Task<DataToken<ICompanyProfileAndNewOfferData>> SaveRegistrationDataAsync(
            ICompanyProfileAndNewOfferData viewModel)
        {
            await UploadImageIfChanged(viewModel);

            var token = await dataTokensService.RegisterNewTokenAsync(viewModel);
            return token;
        }

        private async Task UploadImageIfChanged(ICompanyProfileAndNewOfferData viewModel)
        {
            var file = viewModel.CompanyProfileData.ProfileImage.File;
            if (file != null)
            {
                var imageUrl = await UploadImageAsync(viewModel);
                viewModel.CompanyProfileData.ProfileImage.ImageUrl = imageUrl;
            }
        }

        private Task<string> UploadImageAsync(ICompanyProfileAndNewOfferData viewModel)
        {
            var imageFile = viewModel.CompanyProfileData.ProfileImage.File!;
            return temporaryStorage.ChangeImageAsync(null, imageFile!);
        }

        public async Task DeleteSavedDataAsync(string tokenId)
        {
            await DeleteSavedViewModelAsync(tokenId);
            await DeleteSavedImageAsync(tokenId);
        }

        private Task DeleteSavedViewModelAsync(string tokenId)
        {
            return dataTokensService.ExpireTokenAsync(tokenId);
        }

        private async Task DeleteSavedImageAsync(string tokenId)
        {
            var previousToken = await dataTokensService.TryGetTokenAsync(tokenId);
            var previousImageUrl = previousToken.Value.CompanyProfileData.ProfileImage.ImageUrl;
            await temporaryStorage.DeleteImageIfExistsAsync(previousImageUrl);
        }
    }
}

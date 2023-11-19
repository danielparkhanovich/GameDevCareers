using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Register;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob.Temporary;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.PL.ViewModels.Models.Registration;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Models.Profile.Company;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.PL.Interactors.Notifications;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class EmailCompanyPublishOfferAndRegistrationInteractor
    {
        private readonly IOfferManager offerManager;
        private readonly IUserProfileImagesTemporaryStorage temporaryStorage;
        private readonly DataTokensService<CompanyProfileAndNewOfferData> dataTokensService;
        private readonly ConfirmationTokensService confirmationTokensService;
        private readonly EmailCompanyPublishOfferAndRegistrationService registrationService;


        public EmailCompanyPublishOfferAndRegistrationInteractor(
            IOfferManager offerManager,
            IUserProfileImagesTemporaryStorage temporaryStorage,
            DataTokensService<CompanyProfileAndNewOfferData> dataTokensService,
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
        public async Task<string> SavePostFormAsync(
            CompanyPublishOfferAndRegisterViewModel viewModel, string? previousTokenId = null)
        {
            var form = new CompanyProfileAndNewOfferData()
            {
                CompanyProfileData = viewModel.CompanyProfileData,
                OfferData = viewModel.EditOffer.OfferDetails
            };

            if (!string.IsNullOrEmpty(previousTokenId))
            {
                await DeleteSavedDataAsync(previousTokenId);
            }

            var token = await SaveRegistrationDataAsync(form);
            return token.Id;
        }

        public async Task<CompanyPublishOfferAndRegisterViewModel> GetPostFormViewModelAsync(string tokenId)
        {
            var viewModel = new CompanyPublishOfferAndRegisterViewModel();

            var token = await dataTokensService.TryGetTokenAsync(tokenId);
            var postForm = token.Value;

            viewModel.EditOffer = new EditOfferViewModel()
            {
                OfferDetails = postForm.OfferData
            };

            viewModel.CompanyProfileData = new CompanyProfileViewModel()
            {
                CompanyName = postForm.CompanyProfileData.CompanyName,
                OfficeCity = postForm.CompanyProfileData.OfficeCity,
                CompanyWebsiteUrl = postForm.CompanyProfileData.CompanyWebsiteUrl,
                OfficeCountry = postForm.CompanyProfileData.OfficeCountry,
                OfficeStreet = postForm.CompanyProfileData.OfficeStreet,
                ProfileImage = postForm.CompanyProfileData.ProfileImage,
            };

            return viewModel;
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
            CompanyRegisterViewModel registrationData, string tokenId, ITempDataDictionary tempData)
        {
            try
            {
                await registrationService.TrySendConfirmationTokenAsync(
                    registrationData.Email, registrationData.Password, tokenId);

                NotificationsManager.Instance.SetActionDoneEmailNotification(
                    NotificationsManager.RegisterSection,
                    $"Check your email inbox {registrationData.Email} for a confirmation link to complete your registration.",
                    tempData);
            }
            catch (AuthenticationException e)
            {
                NotificationsManager.Instance.SetErrorNotification(
                    NotificationsManager.RegisterSection,
                    $"Email is already registered.",
                    tempData);
            }
            return RedirectData.NoRedirect;
        }

        /// <returns>Token Id to access data from cache</returns>
        public async Task<string> FinishRegistrationAsync(string tokenId, HttpContext httpContext)
        {
            await registrationService.TryRegisterByTokenAsync(tokenId, httpContext);
            return (await confirmationTokensService.TryGetTokenAsync(tokenId)).TokenToConfirmId;
        }

        public async Task DeletePreviousSavedDataAsync(
            CompanyPublishOfferAndRegisterViewModel registerData, 
            string tokenId)
        {
            await DeleteSavedViewModelAsync(tokenId);

            bool isNewImageAdded = registerData.CompanyProfileData.ProfileImage.File != null;
            if (isNewImageAdded)
            {
                await DeleteSavedImageAsync(tokenId);
            }
        }

        private async Task<DataToken<CompanyProfileAndNewOfferData>> SaveRegistrationDataAsync(
            CompanyProfileAndNewOfferData form)
        {
            await UploadImageIfChanged(form);

            var token = await dataTokensService.RegisterNewTokenAsync(form);
            return token;
        }

        private async Task UploadImageIfChanged(CompanyProfileAndNewOfferData viewModel)
        {
            var file = viewModel.CompanyProfileData.ProfileImage.File;
            if (file != null)
            {
                var imageUrl = await UploadImageAsync(viewModel);
                viewModel.CompanyProfileData.ProfileImage.ImageUrl = imageUrl;
            }
        }

        private Task<string> UploadImageAsync(CompanyProfileAndNewOfferData viewModel)
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

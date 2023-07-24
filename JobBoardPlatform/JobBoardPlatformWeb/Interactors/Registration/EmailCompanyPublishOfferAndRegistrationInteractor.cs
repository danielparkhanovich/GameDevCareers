using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Register;
using JobBoardPlatform.DAL.Repositories.Cache.Tokens;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class EmailCompanyPublishOfferAndRegistrationInteractor
    {
        private readonly DataTokensService<ICompanyProfileAndNewOfferData> dataTokensService;
        private readonly ConfirmationTokensService confirmationTokensService;
        private readonly EmailCompanyPublishOfferAndRegistrationService registrationService;


        public EmailCompanyPublishOfferAndRegistrationInteractor(
            DataTokensService<ICompanyProfileAndNewOfferData> dataTokensService,
            EmailCompanyPublishOfferAndRegistrationService registrationService)
        {
            this.dataTokensService = dataTokensService;
            this.registrationService = registrationService;
        }

        /// <returns>Token Id to access data from cache</returns>
        public async Task<string> SavePostFormViewModel(
            ICompanyProfileAndNewOfferData viewModel, string? previousTokenId = null)
        {
            if (!string.IsNullOrEmpty(previousTokenId))
            {
                await DeleteSavedViewModel(previousTokenId);
            }
            var token = await dataTokensService.RegisterNewTokenAsync(viewModel);
            return token.Id;
        }

        public async Task<ICompanyProfileAndNewOfferData> GetPostFormViewModel(string tokenId)
        {
            var token = await dataTokensService.TryGetTokenAsync(tokenId);
            return token.Value;
        }

        public async Task<bool> IsFormDataConfirmed(string tokenId)
        {
            var token = await dataTokensService.TryGetTokenAsync(tokenId);
            return token.IsConfirmed;
        }

        public async Task<RedirectData> ProcessRegistrationAndRedirect(
            CompanyRegisterViewModel registrationData, string tokenId)
        {
            await registrationService.TrySendConfirmationTokenAndPasswordAsync(
                registrationData.Email, registrationData.Password, tokenId);

            await DeleteSavedViewModel(tokenId);
            return RedirectData.NoRedirect;
        }

        /// <returns>Token Id to access data from cache</returns>
        public async Task<string> FinishRegistration(string tokenId, HttpContext httpContext)
        {
            await registrationService.TryRegisterByTokenAsync(tokenId, httpContext);
            return (await confirmationTokensService.TryGetTokenAsync(tokenId)).TokenToConfirmId;
        }

        private Task DeleteSavedViewModel(string tokenId)
        {
            return dataTokensService.ExpireTokenAsync(tokenId);
        }
    }
}

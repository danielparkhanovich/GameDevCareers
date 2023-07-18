using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Registration;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class CompanyPublishOfferAndRegistrationInteractor
    {
        private readonly DataTokensService<ICompanyProfileAndNewOfferData> dataTokensService;
        private readonly IEmailCompanyRegistrationService registrationService;


        public CompanyPublishOfferAndRegistrationInteractor(
            DataTokensService<ICompanyProfileAndNewOfferData> dataTokensService,
            IEmailCompanyRegistrationService registrationService)
        {
            this.dataTokensService = dataTokensService;
            this.registrationService = registrationService;
        }

        /// <returns>Token Id to access data from cache</returns>
        public async Task<string> SavePostFormViewModel(ICompanyProfileAndNewOfferData viewModel, string? previousTokenId = null)
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
            try
            {
                var token = await dataTokensService.TryGetTokenAsync(tokenId);
                return token.Value;
            }
            catch (TokenValidationException e)
            {
                return new CompanyPublishOfferAndRegisterViewModel();
            }
        }

        private Task DeleteSavedViewModel(string tokenId)
        {
            return dataTokensService.ExpireTokenAsync(tokenId);
        }
    }
}

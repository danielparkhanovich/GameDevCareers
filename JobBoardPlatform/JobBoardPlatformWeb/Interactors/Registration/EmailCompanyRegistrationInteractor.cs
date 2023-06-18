using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.PL.Controllers.Register;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class EmailCompanyRegistrationInteractor : IRegistrationInteractor<CompanyRegisterViewModel>
    {
        private readonly IEmailCompanyRegistrationService registrationService;


        public EmailCompanyRegistrationInteractor(IEmailCompanyRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        public async Task<RedirectData> ProcessRegistrationAndRedirect(CompanyRegisterViewModel model)
        {
            await registrationService.TrySendConfirmationTokenAndPasswordAsync(model.Email);
            return new RedirectData()
            {
                ActionName = BaseRegisterController<UserRegisterViewModel>.CheckVerifyingTokenAction
            };
        }

        public Task FinishRegistration(string tokenId, HttpContext httpContext)
        {
            return registrationService.TryRegisterByTokenAsync(tokenId, httpContext);
        }
    }
}

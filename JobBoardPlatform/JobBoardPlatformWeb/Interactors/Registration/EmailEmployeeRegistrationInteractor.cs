using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.PL.Controllers.Register;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class EmailEmployeeRegistrationInteractor : IRegistrationInteractor<UserRegisterViewModel>
    {
        private readonly IEmailEmployeeRegistrationService registrationService;


        public EmailEmployeeRegistrationInteractor(IEmailEmployeeRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        public async Task<RedirectData> ProcessRegistrationAndRedirect(UserRegisterViewModel model)
        {
            await registrationService.TrySendConfirmationTokenAsync(model.Email, model.Password);
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

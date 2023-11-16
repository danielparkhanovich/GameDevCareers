using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.PL.Controllers.Register;

namespace JobBoardPlatform.PL.Interactors.Registration
{
    public class DefaultRegistrationInteractor<TData, TEntity> : IRegistrationInteractor<TData> 
        where TData : UserLoginData
        where TEntity : class, IUserIdentityEntity, new()
    {
        private readonly IRegistrationService<TEntity> registrationService;
        private readonly IPasswordHasher passwordHasher;


        public DefaultRegistrationInteractor(
            IRegistrationService<TEntity> registrationService,
            IPasswordHasher passwordHasher)
        {
            this.registrationService = registrationService;
            this.passwordHasher = passwordHasher;
        }

        public async Task<RedirectData> ProcessRegistrationAndRedirect(TData model)
        {
            await registrationService.TryRegisterAsync(model.Email, model.Password);
            return new RedirectData()
            {
                ActionName = BaseRegisterController<TData>.TryConfirmRegistrationAction,
                Data = new { email = model.Email, passwordHash = passwordHasher.GetHash(model.Password) }
            };
        }

        public Task FinishRegistration(string tokenId, HttpContext httpContext)
        {
            return Task.CompletedTask;
        }
    }
}

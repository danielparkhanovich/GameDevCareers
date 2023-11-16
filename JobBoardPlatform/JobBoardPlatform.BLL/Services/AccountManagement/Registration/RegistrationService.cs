using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;

namespace JobBoardPlatform.BLL.Services.Authentification.Register
{
    public class RegistrationService<TEntity> : IRegistrationService<TEntity> 
        where TEntity : class, IUserIdentityEntity, new()
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly UserManager<TEntity> userManager;


        public RegistrationService(
            IPasswordHasher passwordHasher,
            UserManager<TEntity> userManager)
        {
            this.passwordHasher = passwordHasher;
            this.userManager = userManager;
        }

        public async Task<TEntity> TryRegisterAsync(string email, string password)
        {
            if (await userManager.IsExistsWithEmailAsync(email))
            {
                throw new AuthenticationException(AuthenticationException.WrongEmail);
            }

            string passwordHash = passwordHasher.GetHash(password);
            var user = GetUserIdentity(email, passwordHash);

            await userManager.AddAsync(user);
            return await userManager.GetWithEmailAsync(email);
        }

        private TEntity GetUserIdentity(string email, string passwordHash)
        {
            var user = new TEntity();
            user.Email = email;
            user.HashPassword = passwordHash;

            if (typeof(TEntity) == typeof(EmployeeIdentity))
            {
                (user as EmployeeIdentity)!.Profile = new EmployeeProfile();
            }
            else if (typeof(TEntity) == typeof(CompanyIdentity))
            {
                (user as CompanyIdentity)!.Profile = new CompanyProfile();
            }

            return user;
        }
    }
}

using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    internal class ValidateCredentials<T> : IValidateCredentials 
        where T: class, ICredentialEntity
    {
        private readonly IRepository<T> repository;


        public ValidateCredentials(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public async Task<AuthorizationResult> ValidateRegisterAsync(string email)
        {
            var result = new AuthorizationResult();

            var set = await repository.GetAllSet();

            var found = await set.FirstOrDefaultAsync(x => x.Email == email);
            if (found != null)
            {
                result.Error = "Email is already registered";
            }

            return result;
        }

        public async Task<AuthorizationResult> ValidateLoginAsync(string email, string hashedPassword)
        {
            var result = new AuthorizationResult();

            var set = await repository.GetAllSet();

            var found = await set.FirstOrDefaultAsync(x => x.Email == email);
            if (found == null)
            {
                result.Error = "Email doesn't exist";
                return result;
            }
            else if (found.HashPassword != hashedPassword)
            {
                result.Error = "Wrong password";
                return result;
            }

            return result;
        }
    }
}

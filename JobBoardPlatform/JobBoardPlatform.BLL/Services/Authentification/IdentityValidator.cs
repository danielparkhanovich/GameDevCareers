using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    internal class IdentityValidator<T> : IIdentityValidator<T>
        where T: class, IUserIdentityEntity
    {
        public AuthentificationResult ValidateRegister(T? user)
        {
            var result = new AuthentificationResult();

            if (user != null)
            {
                result.Error = "Email is already registered";
                return result;
            }

            return AuthentificationResult.Success;
        }

        public AuthentificationResult ValidateLogin(T? user, string hashedPassword)
        {
            var result = new AuthentificationResult();

            if (user == null)
            {
                result.Error = "Email doesn't exist";
                return result;
            }
            else if (user.HashPassword != hashedPassword)
            {
                result.Error = "Wrong password";
                return result;
            }

            return AuthentificationResult.Success;
        }
    }
}

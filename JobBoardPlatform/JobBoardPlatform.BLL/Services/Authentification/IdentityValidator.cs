using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    internal class IdentityValidator<T> : IIdentityValidator<T>
        where T : class, IUserIdentityEntity
    {
        public void ValidateRegisterAsync(T? user)
        {
            if (user != null)
            {
                throw new AuthentificationException(AuthentificationException.EmailAlreadyRegistered);
            }
        }

        public void ValidateLoginAsync(T? user, string hashedPassword)
        {
            if (user == null)
            {
                throw new AuthentificationException(AuthentificationException.EmailNotFound);
            }
            else if (hashedPassword != user.HashPassword)
            {
                throw new AuthentificationException(AuthentificationException.WrongPassword);
            }
        }
    }
}

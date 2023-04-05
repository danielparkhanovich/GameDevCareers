using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    internal interface IIdentityValidator<T>
        where T : class, IUserIdentityEntity
    {
        AuthentificationResult ValidateRegister(T? user);
        AuthentificationResult ValidateLogin(T? user, string hashedPassword);
    }
}

using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    internal interface ICredentialsValidator<T>
        where T : class, ICredentialEntity
    {
        AuthentificationResult ValidateRegister(T? user);
        AuthentificationResult ValidateLogin(T? user, string hashedPassword);
    }
}

using JobBoardPlatform.BLL.Utilities;

namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    internal interface IPasswordHasher
    {
        string HashPassword(string password);
        AuthorizationResult VerifyHashedPassword(string providedPassword, string hashedPassword);
    }
}

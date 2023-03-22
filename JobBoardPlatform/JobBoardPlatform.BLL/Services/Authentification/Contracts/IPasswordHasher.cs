namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    internal interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}

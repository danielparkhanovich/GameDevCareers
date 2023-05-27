namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    internal interface IPasswordHasher
    {
        string GetHash(string password);
    }
}

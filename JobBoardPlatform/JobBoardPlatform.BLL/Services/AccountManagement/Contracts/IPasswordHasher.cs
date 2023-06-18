namespace JobBoardPlatform.BLL.Services.Authentification.Contracts
{
    public interface IPasswordHasher
    {
        string GetHash(string password);
    }
}

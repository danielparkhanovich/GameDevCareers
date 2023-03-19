using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    public class AuthentificationResult
    {
        public static AuthentificationResult Success => new AuthentificationResult();


        public ICredentialEntity? FoundRecord { get; set; }
        public string? Error { get; set; }
        public bool IsError { get => Error != null; }
    }
}

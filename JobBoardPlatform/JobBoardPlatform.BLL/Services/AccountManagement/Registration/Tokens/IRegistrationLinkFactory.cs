
namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    public interface IRegistrationLinkFactory
    {
        public string CreateConfirmationLink(string tokenId);
    }
}

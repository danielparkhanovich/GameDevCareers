
namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    public interface IConfirmationLinkFactory
    {
        public string CreateConfirmationLink(string tokenId);
    }
}

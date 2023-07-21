
namespace JobBoardPlatform.DAL.Repositories.Cache.Tokens
{
    public class ConfirmationToken : IToken
    {
        public string Id { get; set; }
        public string TokenToConfirmId { get; set; }
    }
}

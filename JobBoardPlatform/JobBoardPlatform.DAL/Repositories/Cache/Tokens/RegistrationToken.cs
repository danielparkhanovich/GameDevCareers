
namespace JobBoardPlatform.DAL.Repositories.Cache.Tokens
{
    public class RegistrationToken : IToken
    {
        public string Id { get; set; }
        public string RelatedLogin { get; set; }
        public string PasswordHash { get; set; }
    }
}

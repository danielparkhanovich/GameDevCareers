using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authentification.Authorization.IdentityProviders
{
    public class GitHubProviderClaimKeys : IIdentityProviderClaimKeys
    {
        public string Email => ClaimTypes.Email;

        public string? Name => "urn:github:name";

        public string? Surname => ClaimTypes.Surname;

        public string? Location => "locale";

        public string? UserImageUrl => "picture";

        public string? Bio => null;
    }
}

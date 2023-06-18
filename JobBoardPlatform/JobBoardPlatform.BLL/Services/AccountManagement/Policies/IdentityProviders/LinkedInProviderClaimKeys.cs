using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authentification.Authorization.IdentityProviders
{
    public class LinkedInProviderClaimKeys : IIdentityProviderClaimKeys
    {
        public string Email => ClaimTypes.Email;

        public string? Name => ClaimTypes.GivenName;

        public string? Surname => ClaimTypes.Surname;

        public string? Location => "locale";

        public string? UserImageUrl => "picture";

        public string? Bio => null;
    }
}

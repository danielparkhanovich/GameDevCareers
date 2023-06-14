using System.Security.Claims;

namespace JobBoardPlatform.BLL.Services.Authorization.Policies.IdentityProviders
{
    public class FacebookProviderClaimKeys : IIdentityProviderClaimKeys
    {
        public string Email => ClaimTypes.Email;

        public string? Name => ClaimTypes.GivenName;

        public string? Surname => ClaimTypes.Surname;

        public string? Location => "locale";

        public string? UserImageUrl => "picture";

        public string? Bio => null;
    }
}

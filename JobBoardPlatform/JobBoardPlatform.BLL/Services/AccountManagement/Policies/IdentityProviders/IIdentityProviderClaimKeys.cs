namespace JobBoardPlatform.BLL.Services.Authentification.Authorization.IdentityProviders
{
    public interface IIdentityProviderClaimKeys
    {
        string Email { get; }
        string? Name { get; }
        string? Surname { get; }
        string? Location { get; }
        string? UserImageUrl { get; }
        string? Bio { get; }
    }
}

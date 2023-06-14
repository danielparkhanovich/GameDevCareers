namespace JobBoardPlatform.BLL.Services.Authorization.Policies.IdentityProviders
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

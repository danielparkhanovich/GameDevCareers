namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public static class AuthorizationPolicies
    {
        public const string USER_ONLY_POLICY = $"{Roles.USER}{ONLY_POLICY}";
        public const string COMPANY_ONLY_POLICY = $"{Roles.COMPANY}{ONLY_POLICY}";
        public const string ADMIN_ONLY_POLICY = $"{Roles.ADMIN}{ONLY_POLICY}";

        private const string ONLY_POLICY = "Only";
    }
}

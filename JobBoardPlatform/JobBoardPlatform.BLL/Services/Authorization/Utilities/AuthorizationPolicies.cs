namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public static class AuthorizationPolicies
    {
        public const string EMPLOYEE_ONLY_POLICY = $"{UserRoles.EMPLOYEE}{ONLY_POLICY}";
        public const string COMPANY_ONLY_POLICY = $"{UserRoles.COMPANY}{ONLY_POLICY}";
        public const string ADMIN_ONLY_POLICY = $"{UserRoles.ADMIN}{ONLY_POLICY}";

        private const string ONLY_POLICY = "Only";
    }
}

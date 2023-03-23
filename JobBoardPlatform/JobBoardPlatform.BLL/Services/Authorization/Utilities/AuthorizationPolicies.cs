namespace JobBoardPlatform.BLL.Services.Authorization.Utilities
{
    public static class AuthorizationPolicies
    {
        public const string EmployeeOnlyPolicy = $"{UserRoles.Employee}{OnlyPolicy}";
        public const string CompanyOnlyPolicy = $"{UserRoles.Company}{OnlyPolicy}";
        public const string AdminOnlyPolicy = $"{UserRoles.Admin}{OnlyPolicy}";

        private const string OnlyPolicy = "Only";
    }
}

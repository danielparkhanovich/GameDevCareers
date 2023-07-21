using JobBoardPlatform.BLL.Services.Authentification.Authorization;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Login;
using JobBoardPlatform.BLL.Services.Session;
using JobBoardPlatform.PL.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace JobBoardPlatform.PL.Configuration
{
    public static class AuthorizationServiceExtensions
    {
        public static void AddAuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                AddAuthorizationPolicies(options);
            });

            services.AddTransient(typeof(IAuthorizationService<,>), typeof(AuthorizationService<,>));
            services.AddTransient(typeof(IUserSessionService<,>), typeof(UserSessionService<,>));
            services.AddTransient(typeof(ILoginService<,>), typeof(LoginService<,>));

            AddAuthorizationHandlers(services);
        }

        private static void AddAuthorizationPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(AuthorizationPolicies.EmployeeOnlyPolicy, policy => policy.RequireRole(UserRoles.Employee, UserRoles.Admin));
            options.AddPolicy(AuthorizationPolicies.CompanyOnlyPolicy, policy => policy.RequireRole(UserRoles.Company, UserRoles.Admin));
            options.AddPolicy(AuthorizationPolicies.AdminOnlyPolicy, policy => policy.RequireRole(UserRoles.Admin));
            options.AddPolicy(AuthorizationPolicies.OfferOwnerOnlyPolicy, policy =>
            {
                policy.RequireRole(UserRoles.Company, UserRoles.Admin);
                policy.AddRequirements(new OfferOwnerOrAdminRequirement("offerId"));
            });
            options.AddPolicy(AuthorizationPolicies.OfferPublishedOrOwnerOnlyPolicy, policy =>
            {
                policy.AddRequirements(new OfferPublishedOrOwnerOrAdminRequirement("id"));
            });
        }

        private static void AddAuthorizationHandlers(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IAuthorizationHandler, OfferOwnerHandler>();
            services.AddTransient<IAuthorizationHandler, OfferPublishedOrOwnerHandler>();
        }
    }
}

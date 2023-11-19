using AspNet.Security.OAuth.GitHub;
using AspNet.Security.OAuth.LinkedIn;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Register;
using JobBoardPlatform.DAL.Models.Employee;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Configuration
{
    public static class AuthenticationServiceExtensions
    {
        public static void AddAuthenticationServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            AddAuthenticationCore(services, configuration);
            services.AddTransient<IAuthenticationWithProviderService<EmployeeIdentity>, EmployeeAuthenticationWithProviderService>();
        }

        private static void AddAuthenticationCore(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    AddCookieAuthentication(options);
                })
                .AddGoogle(options =>
                {
                    AddGoogleAuthentication(options, configuration);
                })
                .AddFacebook(options =>
                {
                    AddFacebookAuthentication(options, configuration);
                })
                .AddGitHub(options =>
                {
                    AddGitHubAuthentication(options, configuration);
                })
                .AddLinkedIn(options =>
                {
                    AddLinkedInAuthentication(options, configuration);
                }
            );
        }

        private static void AddCookieAuthentication(CookieAuthenticationOptions options)
        {
            options.LoginPath = "/Login";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        }

        private static void AddGoogleAuthentication(GoogleOptions options, ConfigurationManager configuration)
        {
            options.ClientId = configuration.GetValue<string>("Authentication:Google:ClientId");
            options.ClientSecret = configuration.GetValue<string>("Authentication:Google:ClientSecret");
            options.Scope.Add("profile");
            options.ClaimActions.MapJsonKey("picture", "picture");
        }

        private static void AddFacebookAuthentication(FacebookOptions options, ConfigurationManager configuration)
        {
            options.AppId = configuration.GetValue<string>("Authentication:Facebook:ClientId");
            options.AppSecret = configuration.GetValue<string>("Authentication:Facebook:ClientSecret");
            options.Fields.Add("picture");

            options.Events = new OAuthEvents
            {
                OnCreatingTicket = context =>
                {
                    var identity = (ClaimsIdentity)context.Principal!.Identity!;
                    var profileImg = context.User
                        .GetProperty("picture").GetProperty("data").GetProperty("url").ToString();

                    string nameIdentifier = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    string thumbnailUrl = $"https://graph.facebook.com/{nameIdentifier}/picture?type=large";

                    identity.AddClaim(new Claim("picture", thumbnailUrl));
                    return Task.CompletedTask;
                }
            };
        }

        private static void AddGitHubAuthentication(GitHubAuthenticationOptions options, ConfigurationManager configuration)
        {
            options.ClientId = configuration.GetValue<string>("Authentication:GitHub:ClientId");
            options.ClientSecret = configuration.GetValue<string>("Authentication:GitHub:ClientSecret");
            options.Scope.Add("user:email");
            options.Scope.Add("read:user");
        }

        private static void AddLinkedInAuthentication(LinkedInAuthenticationOptions options, ConfigurationManager configuration)
        {
            options.ClientId = configuration.GetValue<string>("Authentication:LinkedIn:ClientId");
            options.ClientSecret = configuration.GetValue<string>("Authentication:LinkedIn:ClientSecret");
        }
    }
}

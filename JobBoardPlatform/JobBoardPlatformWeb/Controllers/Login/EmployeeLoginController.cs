using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using Microsoft.AspNetCore.Authentication.Facebook;
using AspNet.Security.OAuth.GitHub;
using AspNet.Security.OAuth.LinkedIn;
using JobBoardPlatform.BLL.Services.Authorization.Policies.IdentityProviders;

namespace JobBoardPlatform.PL.Controllers.Login
{
    [Route("signin-employee")]
    public class EmployeeLoginController : BaseLoginController<EmployeeIdentity, EmployeeProfile>
    {
        public const string LoginWithGoogleAction = "LoginWithGoogle";
        public const string LoginWithFacebookAction = "LoginWithFacebook";
        public const string LoginWithGitHubAction = "LoginWithGitHub";
        public const string LoginWithLinkedInAction = "LoginWithLinkedIn";

        private readonly IIdentityServiceWithProvider<EmployeeIdentity> identityService;


        public EmployeeLoginController(
            IIdentityServiceWithProvider<EmployeeIdentity> identityService,
            IRepository<EmployeeIdentity> credentialsRepository,
            IRepository<EmployeeProfile> profileRepository)
        {
            this.identityService = identityService;
            this.credentialsRepository = credentialsRepository;
            this.profileRepository = profileRepository;
        }

        protected override EmployeeIdentity GetIdentity(UserLoginViewModel userLogin)
        {
            var credentials = new EmployeeIdentity()
            {
                Email = userLogin.Email,
                HashPassword = userLogin.Password
            };

            return credentials;
        }

        [HttpGet]
        [Route("google")]
        public Task LoginWithGoogle()
        {
            return ChallengeAsync(
                GoogleDefaults.AuthenticationScheme, 
                "LoginWithGoogleCallback");
        }

        [HttpGet]
        [Route("facebook")]
        public Task LoginWithFacebook()
        {
            return ChallengeAsync(
                FacebookDefaults.AuthenticationScheme, 
                "LoginWithFacebookCallback");
        }

        [HttpGet]
        [Route("github")]
        public Task LoginWithGitHub()
        {
            return ChallengeAsync(
                GitHubAuthenticationDefaults.AuthenticationScheme, 
                "LoginWithGitHubCallback");
        }

        [HttpGet]
        [Route("linkedin")]
        public Task LoginWithLinkedIn()
        {
            return ChallengeAsync(
                LinkedInAuthenticationDefaults.AuthenticationScheme, 
                "LoginWithLinkedInCallback");
        }

        [Route("google-callback")]
        public Task<IActionResult> LoginWithGoogleCallback()
        {
            return TryLoginOrRegisterAsync(new GoogleProviderClaimKeys());
        }

        [Route("facebook-callback")]
        public Task<IActionResult> LoginWithFacebookCallback()
        {
            return TryLoginOrRegisterAsync(new FacebookProviderClaimKeys());
        }

        [Route("github-callback")]
        public Task<IActionResult> LoginWithGitHubCallback()
        {
            return TryLoginOrRegisterAsync(new GitHubProviderClaimKeys());
        }

        [Route("linkedin-callback")]
        public Task<IActionResult> LoginWithLinkedInCallback()
        {
            return TryLoginOrRegisterAsync(new LinkedInProviderClaimKeys());
        }

        private async Task ChallengeAsync(string scheme, string callback)
        {
            var redirectUrl = Url.Action(callback, "EmployeeLogin", new { area = "", route = "signin-employee" });
            await HttpContext.ChallengeAsync(
                scheme, new AuthenticationProperties()
                {
                    RedirectUri = redirectUrl
                }
            );
        }

        private async Task<IActionResult> TryLoginOrRegisterAsync(IIdentityProviderClaimKeys claimKeys)
        {

            await identityService.TryLoginOrRegisterAsync(HttpContext, claimKeys);
            return RedirectToAction("Index", "Home");
        }
    }
}

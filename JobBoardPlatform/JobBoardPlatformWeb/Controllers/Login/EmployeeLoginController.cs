using JobBoardPlatform.DAL.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using AspNet.Security.OAuth.GitHub;
using AspNet.Security.OAuth.LinkedIn;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.Contracts;
using JobBoardPlatform.BLL.Services.Authentification.Authorization.IdentityProviders;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;

namespace JobBoardPlatform.PL.Controllers.Login
{
    [Route("signin-employee")]
    public class EmployeeLoginController : BaseLoginController<EmployeeIdentity, EmployeeProfile>
    {
        private readonly IAuthenticationWithProviderService<EmployeeIdentity> identityService;


        public EmployeeLoginController(
            IAuthenticationWithProviderService<EmployeeIdentity> identityService,
            ILoginService<EmployeeIdentity, EmployeeProfile> loginService) 
            : base(loginService)
        {
            this.identityService = identityService;
        }

        [HttpGet]
        [Route("google")]
        public Task LoginWithGoogle()
        {
            return ChallengeAsync(
                GoogleDefaults.AuthenticationScheme,
                nameof(LoginWithGoogleCallback));
        }

        [HttpGet]
        [Route("facebook")]
        public Task LoginWithFacebook()
        {
            return ChallengeAsync(
                FacebookDefaults.AuthenticationScheme,
                nameof(LoginWithFacebookCallback));
        }

        [HttpGet]
        [Route("github")]
        public Task LoginWithGitHub()
        {
            return ChallengeAsync(
                GitHubAuthenticationDefaults.AuthenticationScheme,
                nameof(LoginWithGitHubCallback));
        }

        [HttpGet]
        [Route("linkedin")]
        public Task LoginWithLinkedIn()
        {
            return ChallengeAsync(
                LinkedInAuthenticationDefaults.AuthenticationScheme,
                nameof(LoginWithLinkedInCallback));
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
            var redirectUrl = Url.Action(callback);
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

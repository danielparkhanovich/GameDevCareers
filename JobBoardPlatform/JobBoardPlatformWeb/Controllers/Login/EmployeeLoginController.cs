using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using JobBoardPlatform.BLL.Services.Authentification.Common;
using JobBoardPlatform.BLL.Services.Authorization.Contracts;
using Microsoft.AspNetCore.Authentication.Facebook;
using AspNet.Security.OAuth.GitHub;
using AspNet.Security.OAuth.LinkedIn;

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
        public async Task LoginWithGoogle()
        {
            var redirectUrl = Url.Action("LoginWithGoogleCallback");
            await HttpContext.ChallengeAsync(
                GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = redirectUrl
            });
        }

        [HttpGet]
        [Route("facebook")]
        public async Task LoginWithFacebook()
        {
            var redirectUrl = Url.Action("LoginWithGoogleCallback");
            await HttpContext.ChallengeAsync(
                FacebookDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = redirectUrl
            });
        }

        [HttpGet]
        [Route("gitHub")]
        public async Task LoginWithGitHub()
        {
            var redirectUrl = Url.Action("LoginWithGoogleCallback");
            await HttpContext.ChallengeAsync(
                GitHubAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = redirectUrl
            });
        }

        [HttpGet]
        [Route("linkedIn")]
        public async Task LoginWithLinkedIn()
        {
            var redirectUrl = Url.Action("LoginWithGoogleCallback");
            await HttpContext.ChallengeAsync(
                LinkedInAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = redirectUrl
            });
        }

        private async Task ChallengeAsync(string scheme)
        {

        }

        [Route("google-callback")]
        public async Task<IActionResult> LoginWithGoogleCallback()
        {
            await identityService.TryLoginOrRegisterAsync(HttpContext, AuthentificationValues.GoogleProfileImageKey);
            return RedirectToAction("Index", "Home");
        }
    }
}

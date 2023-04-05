using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.PL.ViewModels.Authentification;
using NuGet.Protocol.Core.Types;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Models;

namespace JobBoardPlatform.PL.Controllers.Login
{
    /*
    ClaimsPrincipal claimUser = httpContext.User;

    if (claimUser.Identity.IsAuthenticated)
    {
        return AuthorizationResult.Success;
    }
    */
    public abstract class BaseLoginController<T, V> : Controller 
        where T: class, IUserIdentityEntity
        where V: class, IUserProfileEntity
    {
        protected IRepository<T> credentialsRepository;
        protected IRepository<V> profileRepository;


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                var credentials = GetIdentity(userLogin);

                return await TryLogin(userLogin, credentials);
            }
            return View(userLogin);
        }

        protected abstract T GetIdentity(UserLoginViewModel userLogin);

        private async Task<IActionResult> TryLogin(UserLoginViewModel userLogin, T credentials)
        {
            var session = new IdentityService<T, V>(HttpContext, credentialsRepository, profileRepository);

            var autorization = await session.TryLoginAsync(credentials);
            if (autorization.IsError)
            {
                ModelState.AddModelError("Autorization error", autorization.Error);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(userLogin);
        }
    }
}

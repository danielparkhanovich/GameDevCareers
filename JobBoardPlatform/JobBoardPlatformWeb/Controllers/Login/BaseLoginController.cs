using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.Filters;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.BLL.Services.Authentification;

namespace JobBoardPlatform.PL.Controllers.Login
{
    [TypeFilter(typeof(SkipLoggedInUsersFilter))]
    public abstract class BaseLoginController<TIdentity, TProfile> : Controller 
        where TIdentity: class, IUserIdentityEntity
        where TProfile: class, IUserProfileEntity
    {
        protected IRepository<TIdentity> credentialsRepository;
        protected IRepository<TProfile> profileRepository;


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

        protected abstract TIdentity GetIdentity(UserLoginViewModel userLogin);

        private async Task<IActionResult> TryLogin(UserLoginViewModel userLogin, TIdentity credentials)
        {
            var authentification = new AuthentificationService<TIdentity>(credentialsRepository);
            var session = new IdentityService<TIdentity, TProfile>(
                HttpContext, authentification, profileRepository);

            try
            {
                await session.TryLoginAsync(credentials);
                return RedirectToAction("Index", "Home");
            }
            catch (AuthentificationException e)
            {
                ModelState.AddModelError("Autorization error", e.Message);
            }

            return View(userLogin);
        }
    }
}

using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Register
{
    [TypeFilter(typeof(SkipLoggedInUsersFilter))]
    public abstract class BaseRegisterController<TIdentity, TProfile, TViewModel> : Controller
        where TIdentity : class, IUserIdentityEntity
        where TProfile : class, IUserProfileEntity
        where TViewModel : class
    {
        protected IRepository<TIdentity> credentialsRepository;
        protected IRepository<TProfile> profileRepository;


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(TViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                var credential = GetIdentity(userRegister);

                return await TryRegister(userRegister, credential);
            }
            return View(userRegister);
        }

        protected abstract TIdentity GetIdentity(TViewModel userLogin);

        private async Task<IActionResult> TryRegister(TViewModel userLogin, TIdentity credentials)
        {
            var session = new IdentityService<TIdentity, TProfile>(HttpContext, credentialsRepository, profileRepository);

            try
            {
                await session.TryRegisterAsync(credentials);
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

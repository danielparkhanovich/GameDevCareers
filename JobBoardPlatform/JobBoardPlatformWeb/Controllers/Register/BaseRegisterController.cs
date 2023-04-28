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

            var autorization = await session.TryRegisterAsync(credentials);
            if (autorization.IsError)
            {
                ModelState.AddModelError("AlreadyExistsError", autorization.Error);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View(userLogin);
        }
    }
}

using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.Authentification;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers.Register
{
    public abstract class BaseRegisterController<T, V> : Controller
        where T : class, IUserIdentityEntity
        where V : class, IUserProfileEntity
    {
        protected IRepository<T> credentialsRepository;
        protected IRepository<V> profileRepository;


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                var credential = GetIdentity(userRegister);

                return await TryRegister(userRegister, credential);
            }
            return View(userRegister);
        }

        protected abstract T GetIdentity(UserRegisterViewModel userLogin);

        private async Task<IActionResult> TryRegister(UserRegisterViewModel userLogin, T credentials)
        {
            var session = new IdentityService<T, V>(HttpContext, credentialsRepository, profileRepository);

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

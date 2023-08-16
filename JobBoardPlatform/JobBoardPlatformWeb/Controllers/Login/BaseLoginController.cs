using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.PL.Filters;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.PL.Interactors.Notifications;
using JobBoardPlatform.PL.ViewModels.Models.MainTechnologyWidgets;

namespace JobBoardPlatform.PL.Controllers.Login
{
    [TypeFilter(typeof(SkipLoggedInUsersFilter))]
    public abstract class BaseLoginController<TIdentity, TProfile> : Controller 
        where TIdentity: class, IUserIdentityEntity
        where TProfile: class, IUserProfileEntity
    {
        private readonly ILoginService<TIdentity, TProfile> loginService;


        public BaseLoginController(ILoginService<TIdentity, TProfile> loginService)
        {
            this.loginService = loginService;
        }

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
                return await TryLogin(userLogin);
            }
            return View(userLogin);
        }

        private async Task<IActionResult> TryLogin(UserLoginViewModel userLogin)
        {
            try
            {
                await loginService.TryLoginAsync(userLogin.Email, userLogin.Password, HttpContext);
                return RedirectToAction("Index", "Home");
            }
            catch (AuthenticationException e)
            {
                NotificationsManager.Instance.SetErrorNotification(
                    NotificationsManager.LoginSection, "Wrong email or password", TempData);
            }

            return View(userLogin);
        }
    }
}

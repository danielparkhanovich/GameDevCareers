using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.PL.Interactors.Notifications;
using JobBoardPlatform.PL.ViewModels.Models.Authentification;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Admin;
using JobBoardPlatform.BLL.Services.Authentification.Exceptions;

namespace JobBoardPlatform.PL.Controllers.Login
{
    [Route("signin-admin")]
    public class AdminLoginController : Controller
    {
        private readonly ILoginService<AdminIdentity> loginService;


        public AdminLoginController(ILoginService<AdminIdentity> loginService)
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

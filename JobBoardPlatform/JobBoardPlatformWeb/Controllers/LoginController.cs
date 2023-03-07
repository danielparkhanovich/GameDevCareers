using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;

namespace JobBoardPlatform.PL.Controllers
{
    /*
    ClaimsPrincipal claimUser = httpContext.User;

    if (claimUser.Identity.IsAuthenticated)
    {
        return AuthorizationResult.Success;
    }
     */
    public class LoginController : Controller
    {
        private readonly IRepository<EmployeeCredentials> employeeRepository;
        private readonly IRepository<CompanyCredentials> companyRepository;


        public LoginController(IRepository<EmployeeCredentials> employeeRepository, IRepository<CompanyCredentials> companyRepository)
        {
            this.employeeRepository = employeeRepository;
            this.companyRepository = companyRepository;
        }

        public IActionResult Employee()
        {
            return View();
        }

        public IActionResult Company()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            var sessionManager = new SessionManager(HttpContext);
            await sessionManager.SignOutHttpContextAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Employee(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                var credential = new EmployeeCredentials()
                {
                    Email = userLogin.Email,
                    HashPassword = userLogin.Password
                };
                return await TryLogin(userLogin, employeeRepository, credential);
            }
            return View(userLogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Company(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                var credential = new CompanyCredentials()
                {
                    Email = userLogin.Email,
                    HashPassword = userLogin.Password
                };
                return await TryLogin(userLogin, companyRepository, credential);
            }
            return View(userLogin);
        }

        private async Task<IActionResult> TryLogin<T>(UserLoginViewModel userLogin, IRepository<T> repository, T credentials)
            where T : class, ICredentialEntity
        {
            var processAutorization = new ProcessAuthentification<T>(repository, HttpContext);

            var autorization = await processAutorization.TryLoginAsync(credentials);
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

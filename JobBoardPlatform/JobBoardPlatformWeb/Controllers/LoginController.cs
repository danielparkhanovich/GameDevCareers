using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;

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
        private readonly IRepository<EmployeeProfile> employeeProfileRepository;
        private readonly IRepository<CompanyProfile> companyProfileRepository;


        public LoginController(IRepository<EmployeeCredentials> employeeRepository, IRepository<CompanyCredentials> companyRepository,
            IRepository<EmployeeProfile> employeeProfileRepository, IRepository<CompanyProfile> companyProfileRepository)
        {
            this.employeeRepository = employeeRepository;
            this.companyRepository = companyRepository;
            this.employeeProfileRepository = employeeProfileRepository;
            this.companyProfileRepository = companyProfileRepository;
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
            var sessionManager = new AuthorizationService(HttpContext);
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

                return await TryLogin(userLogin, 
                    employeeRepository, employeeProfileRepository, credential, Roles.USER);
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

                return await TryLogin(userLogin, 
                    companyRepository, companyProfileRepository, credential, Roles.COMPANY);
            }
            return View(userLogin);
        }

        private async Task<IActionResult> TryLogin<T, V>(UserLoginViewModel userLogin, 
            IRepository<T> credentialRepository, IRepository<V> profileRepository, T credentials, string role)
            where T : class, ICredentialEntity
            where V : class, IProfileEntity
        {
            var session = new SessionService<T, V>(HttpContext, credentialRepository, profileRepository, role);

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

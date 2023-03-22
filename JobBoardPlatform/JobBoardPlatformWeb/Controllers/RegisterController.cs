using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels.Authentification;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRepository<EmployeeCredentials> employeeRepository;
        private readonly IRepository<CompanyCredentials> companyRepository;
        private readonly IRepository<EmployeeProfile> employeeProfileRepository;
        private readonly IRepository<CompanyProfile> companyProfileRepository;


        public RegisterController(IRepository<EmployeeCredentials> employeeRepository, IRepository<CompanyCredentials> companyRepository,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Employee(UserRegisterViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                var credential = new EmployeeCredentials()
                {
                    Email = userRegister.Email,
                    HashPassword = userRegister.Password,
                    Profile = new EmployeeProfile()
                };
                return await TryRegister(userRegister, 
                    employeeRepository, employeeProfileRepository, credential, Roles.USER);
            }
            return View(userRegister);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Company(UserRegisterViewModel userRegister)
        {
            if (ModelState.IsValid)
            {
                var credential = new CompanyCredentials()
                {
                    Email = userRegister.Email,
                    HashPassword = userRegister.Password,
                    Profile = new CompanyProfile()
                };
                return await TryRegister(userRegister,
                    companyRepository, companyProfileRepository, credential, Roles.COMPANY);
            }
            return View(userRegister);
        }

        private async Task<IActionResult> TryRegister<T, V>(UserRegisterViewModel userLogin,
            IRepository<T> credentialRepository, IRepository<V> profileRepository, T credentials, string role)
            where T : class, ICredentialEntity
            where V : class, IEntity, IDisplayData
        {
            var session = new SessionService<T, V>(HttpContext, credentialRepository, profileRepository, role);

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

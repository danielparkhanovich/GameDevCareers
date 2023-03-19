using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRepository<EmployeeCredentials> employeeRepository;
        private readonly IRepository<CompanyCredentials> companyRepository;
        private readonly IRepository<EmployeeProfile> employeeProfiles;
        private readonly IRepository<CompanyProfile> companyProfiles;


        public RegisterController(IRepository<EmployeeCredentials> employeeRepository, IRepository<CompanyCredentials> companyRepository,
                                  IRepository<EmployeeProfile> employeeProfiles, IRepository<CompanyProfile> companyProfiles) 
        {
            this.employeeRepository = employeeRepository;
            this.companyRepository = companyRepository;
            this.employeeProfiles = employeeProfiles;
            this.companyProfiles = companyProfiles;
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
                return await TryRegister(userRegister, employeeRepository, employeeProfiles, credential, Roles.USER);
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
                return await TryRegister(userRegister, companyRepository, companyProfiles, credential, Roles.COMPANY);
            }
            return View(userRegister);
        }

        private async Task<IActionResult> TryRegister<T>(UserRegisterViewModel userRegister, IRepository<T> userRepository, 
            IRepository<T> userProfileRepository, T credentials, string role) 
            where T : class, ICredentialEntity
        {
            var session = new SessionService<T>(HttpContext, employeeRepository, repository, role);

            var autorization = await session.TryRegisterAsync(credentials);
            if (autorization.IsError)
            {
                ModelState.AddModelError("AlreadyExistsError", autorization.Error);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(userRegister);
        }
    }
}

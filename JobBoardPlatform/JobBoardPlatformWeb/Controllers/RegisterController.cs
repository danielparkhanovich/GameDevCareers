using JobBoardPlatform.BLL.Services.Authentification;
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


        public RegisterController(IRepository<EmployeeCredentials> employeeRepository, IRepository<CompanyCredentials> companyRepository) 
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
                return await TryRegister(userRegister, employeeRepository, credential);
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
                return await TryRegister(userRegister, companyRepository, credential);
            }
            return View(userRegister);
        }

        private async Task<IActionResult> TryRegister<T>(UserRegisterViewModel userRegister, IRepository<T> repository, T credentials) 
            where T : class, ICredentialEntity
        {
            var processAutorization = new ProcessAuthentification<T>(repository, HttpContext);

            var autorization = await processAutorization.TryRegisterAsync(credentials);
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

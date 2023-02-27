using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.PL.Controllers
{
    public class SignUpController : Controller
    {
        private IRepository<EmployeeCredentials> employeeRepository;
        private IRepository<CompanyCredentials> companyRepository;


        public SignUpController(IRepository<EmployeeCredentials> employeeRepository, IRepository<CompanyCredentials> companyRepository) 
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
        public async Task<IActionResult> Employee(UserLoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                var credential = new EmployeeCredentials() { 
                    Email = userLogin.Email,
                    HashPassword = userLogin.Password
                };
                return await TryRegister(userLogin, employeeRepository, credential);
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
                return await TryRegister(userLogin, companyRepository, credential);
            }
            return View(userLogin);
        }

        private async Task<IActionResult> TryRegister<T>(UserLoginViewModel userLogin, IRepository<T> repository, T credentials) 
            where T : class, ICredentialEntity
        {
            var processAutorization = new ProcessAutorization<T>(repository);

            var autorization = await processAutorization.TryRegister(credentials);
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

using JobBoardPlatform.BLL.Services.Authorization;
using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Repositories.Contracts;
using JobBoardPlatform.PL.ViewModels.Authentification;
using JobBoardPlatform.PL.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IRepository<EmployeeProfile> employeeProfileRepository;
        private readonly IRepository<EmployeeProfile> companyProfileRepository;


        public ProfileController(IRepository<EmployeeProfile> employeeProfileRepository, 
            IRepository<EmployeeProfile> companyProfileRepository)
        {
            this.employeeProfileRepository = employeeProfileRepository;
            this.companyProfileRepository = companyProfileRepository;
        }

        [Authorize(Policy = AuthorizationPolicies.USER_ONLY_POLICY)]
        public async Task<IActionResult> Employee()
        {
            var viewModel = await GetValidUserViewModel();

            return View(viewModel);
        }

        [Authorize(Policy = AuthorizationPolicies.USER_ONLY_POLICY)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Employee(EmployeeProfileViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(User.FindFirstValue("Id"));
                var profile = await employeeProfileRepository.Get(id);

                if (!string.IsNullOrEmpty(employeeViewModel.Name))
                {
                    profile.Name = employeeViewModel.Name;
                }
                if (!string.IsNullOrEmpty(employeeViewModel.Surname))
                {
                    profile.Surname = employeeViewModel.Surname;
                }
                if (!string.IsNullOrEmpty(employeeViewModel.City))
                {
                    profile.City = employeeViewModel.City;
                }
                if (!string.IsNullOrEmpty(employeeViewModel.Country))
                {
                    profile.Country = employeeViewModel.Country;
                }
                if (!string.IsNullOrEmpty(employeeViewModel.Description))
                {
                    profile.Description = employeeViewModel.Description;
                }
                if (!string.IsNullOrEmpty(employeeViewModel.ResumeUrl))
                {
                    profile.ResumeUrl = employeeViewModel.ResumeUrl;
                }
                if (!string.IsNullOrEmpty(employeeViewModel.PhotoUrl))
                {
                    profile.PhotoUrl = employeeViewModel.PhotoUrl;
                }

                await employeeProfileRepository.Update(profile);

                var sessionManager = new AuthorizationService(HttpContext);
                await sessionManager.SignOutHttpContextAsync();

                var authorization = new AuthorizationData()
                {
                    Id = id,
                    NameIdentifier = profile.Name,
                    DisplayImageUrl = profile.PhotoUrl,
                    DisplayName = profile.Name,
                    Role = Roles.USER
                };

                await sessionManager.SignInHttpContextAsync(authorization);
            }

            employeeViewModel = await GetValidUserViewModel();

            return View(employeeViewModel);
        }

        [Authorize(Policy = AuthorizationPolicies.COMPANY_ONLY_POLICY)]
        public IActionResult Company()
        {
            return View();
        }

        private async Task<EmployeeProfileViewModel> GetValidUserViewModel()
        {
            int id = int.Parse(User.FindFirstValue("Id"));

            var profile = await employeeProfileRepository.Get(id);

            return new EmployeeProfileViewModel()
            {
                Name = profile.Name,
                Surname = profile.Surname,
                City = profile.City,
                Country = profile.Country,
                Description = profile.Description,
                PhotoUrl = profile.PhotoUrl,
                ResumeUrl = profile.ResumeUrl
            };
        }
    }
}

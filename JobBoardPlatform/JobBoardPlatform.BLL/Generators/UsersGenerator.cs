using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Admin;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.BLL.Common;

namespace JobBoardPlatform.BLL.Utils
{
    public class UsersGenerator
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly UserManager<CompanyIdentity> companyManager;
        private readonly UserManager<EmployeeIdentity> employeeManager;
        private readonly UserManager<AdminIdentity> adminManager;


        public UsersGenerator(
            IPasswordHasher passwordHasher,
            UserManager<CompanyIdentity> companyManager,
            UserManager<EmployeeIdentity> employeeManager,
            UserManager<AdminIdentity> adminManager)
        {
            this.passwordHasher = passwordHasher;
            this.companyManager = companyManager;
            this.employeeManager = employeeManager;
            this.adminManager = adminManager;
        }

        public async Task CreateAdmin(string login, string password)
        {
            var email = GetTestEmail(login, "admin");
            string passwordHash = passwordHasher.GetHash(password);

            var admin = new AdminIdentity()
            {
                Email = email,
                HashPassword = passwordHash
            };

            await adminManager.AddAsync(admin);
        }

        public async Task CreateCompany(CompanyProfileData profileData, string password)
        {
            var email = GetTestEmail(profileData.CompanyName!, "company");
            string passwordHash = passwordHasher.GetHash(password);

            var company = new CompanyIdentity()
            {
                Email = email,
                HashPassword = passwordHash,
                Profile = new CompanyProfile(),
            };

            await companyManager.AddAsync(company);

            var user = await companyManager.GetWithEmailAsync(email);
            await companyManager.UpdateProfileAsync(user.Id, profileData);
        }

        public async Task CreateEmployee(EmployeeProfileData profileData, string password)
        {
            var email = GetTestEmail($"{profileData.Name}{profileData.Surname}", "employee");
            string passwordHash = passwordHasher.GetHash(password);

            var employee = new EmployeeIdentity()
            {
                Email = email,
                HashPassword = passwordHash,
                Profile = new EmployeeProfile(),
            };

            await employeeManager.AddAsync(employee);

            var user = await employeeManager.GetWithEmailAsync(email);
            await employeeManager.UpdateProfileAsync(user.Id, profileData);
        }

        public static string GetTestEmail(string suffix, string role)
        {
            var splitted = GlobalBLL.Values.TestEmail.Split('@');
            var suffixFormatted = GetFormattedSuffix(suffix);

            // use same test email for each created user
            return $"{splitted[0]}+generated-{role}-{suffixFormatted}@{splitted[1]}";
        }

        private static string GetFormattedSuffix(string suffix)
        {
            return suffix.Replace(' ', '-').ToLower();
        }
    }
}

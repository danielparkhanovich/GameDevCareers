using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Application;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Utils;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.Controllers.Presenters;
using NuGet.Packaging.Signing;

namespace JobBoardPlatform.BLL.Generators
{
    public class ShowcaseSnapshotGenerator : IShowcaseSnapshotGenerator
    {
        private readonly UsersGenerator usersGenerator;
        private readonly AdminCommands adminCommands;
        private readonly IApplicationsManager applicationsManager;
        private readonly IOfferManager offerManager;
        private readonly UserManager<EmployeeIdentity> employeeManager;
        private readonly ApplicationEmailViewRenderer emailViewRenderer;


        public ShowcaseSnapshotGenerator(
            UsersGenerator usersGenerator, 
            AdminCommands adminCommands,
            IApplicationsManager applicationsManager,
            IOfferManager offerManager,
            UserManager<EmployeeIdentity> employeeManager,
            ApplicationEmailViewRenderer emailViewRenderer)
        {
            this.usersGenerator = usersGenerator;
            this.adminCommands = adminCommands;
            this.applicationsManager = applicationsManager;
            this.offerManager = offerManager;
            this.employeeManager = employeeManager;
            this.emailViewRenderer = emailViewRenderer;
        }

        public async Task CreateCompanies()
        {
            var companies = new List<CompanyProfileData>();

            var company0 = new CompanyProfileData()
            {
                CompanyName = "PixelForge Games",
                OfficeCity = "San Francisco",
                OfficeCountry = "United States",
                OfficeStreet = "Silicon Avenue",
                ProfileImage = GetCompanyImage(0, "jpg"),
                CompanyWebsiteUrl = "http://www.example.com/index.html",
            };

            var company1 = new CompanyProfileData()
            {
                CompanyName = "Some Games",
                OfficeCity = "Tokyo",
                OfficeCountry = "Japan",
                OfficeStreet = "Hikari Street",
                ProfileImage = GetCompanyImage(1, "jpg"),
                CompanyWebsiteUrl = "http://www.example.com/index.html",
            };

            var company2 = new CompanyProfileData()
            {
                CompanyName = "NebulaByte Interactive",
                OfficeCity = "Sydney",
                OfficeCountry = "Australia",
                ProfileImage = GetCompanyImage(2, "png"),
            };

            var company3 = new CompanyProfileData()
            {
                CompanyName = "MysticPulse Entertainment",
                OfficeCity = "Barcelona",
                OfficeCountry = "Spain",
                OfficeStreet = "Enigma Street",
                ProfileImage = GetCompanyImage(3, "png"),
                CompanyWebsiteUrl = "http://www.example.com/index.html",
            };

            var company4 = new CompanyProfileData()
            {
                CompanyName = "CyberNova Creations",
                OfficeCity = "Seoul",
                OfficeCountry = "South Korea",
                OfficeStreet = "Cybernaut Boulevard",
                ProfileImage = GetCompanyImage(4, "png"),
                CompanyWebsiteUrl = "http://www.example.com/index.html",
            };

            var company5 = new CompanyProfileData()
            {
                CompanyName = "ArcaneSparrow Games",
                OfficeCity = "London",
                OfficeCountry = "United Kingdom",
                ProfileImage = GetCompanyImage(5, "jpg"),
            };

            companies.Add(company0);
            companies.Add(company1);
            companies.Add(company2);
            companies.Add(company3);
            companies.Add(company4);
            companies.Add(company5);

            foreach (var company in companies)
            {
                await usersGenerator.CreateCompany(company, "1234567890!");
            }
        }

        public async Task CreateEmployees()
        {
            var employees = new List<EmployeeProfileData>();

            var employee0 = new EmployeeProfileData()
            {
                Name = "Sarah Johnson",
                Surname = "Smith",
                City = "Los Angeles",
                Country = "United States",
                Description = 
                    "Sarah is a passionate game developer with 5 years of experience in game design and programming. " +
                    "She specializes in creating immersive virtual reality experiences and has a strong background in Unity and Unreal Engine. " +
                    "Sarah is currently seeking a new opportunity to contribute her skills to innovative game development projects.\r\n",
                ProfileImage = GetEmployeeImage(),
                File = GetEmployeeResume(),
            };

            var employee1 = new EmployeeProfileData()
            {
                Name = "Diego Martinez",
                Surname = "Gonzalez",
                City = "Barcelona",
                Country = "Spain",
                ProfileImage = GetEmployeeImage(),
                File = GetEmployeeResume(),
            };

            var employee2 = new EmployeeProfileData()
            {
                Name = "Max Fischer",
                Surname = "Schmidt",
                City = "Berlin",
                Country = "Germany",
                Description =
                    "Max is a skilled game programmer with a passion for coding and optimization. " +
                    "He specializes in creating efficient game engines and enhancing graphics performance. " +
                    "Max is looking for a team that values innovation and technical excellence.",
                ProfileImage = GetEmployeeImage(),
                File = GetEmployeeResume(),
            };

            var employee3 = new EmployeeProfileData()
            {
                Name = "Mia Wang",
                Surname = "Li",
                City = "Beijing",
                Country = "China",
                ProfileImage = GetEmployeeImage(),
            };

            var employee4 = new EmployeeProfileData()
            {
                Name = "Lily Chen",
                Surname = "Wu",
                City = "Vancouver",
                Country = "Canada",
                ProfileImage = GetEmployeeImage(),
                File = GetEmployeeResume(),
            };

            employees.Add(employee0);
            employees.Add(employee1);
            employees.Add(employee2);
            employees.Add(employee3);
            employees.Add(employee4);

            foreach (var employee in employees)
            {
                await usersGenerator.CreateEmployee(employee, "1234567890!");
            }
        }

        public async Task CreateAdmins()
        {
            var logins = new List<string>()
            {
                "admin1",
                "admin2",
                "admin3"
            };

            foreach (var login in logins)
            {
                await usersGenerator.CreateAdmin(login, "1234567890!");
            }
        }

        public async Task CreateOffers(int offersCount)
        {
            await adminCommands.GenerateOffers(0, offersCount);

            var offersIds = await offerManager.GetAllIdsAsync();

            // shuffle for random order
            offersIds = offersIds.OrderBy(a => Guid.NewGuid()).ToList();

            foreach (var offerId in offersIds) 
            {
                await offerManager.PassPaymentAsync(offerId);
            }
        }

        public async Task CreateApplications()
        {
            var applications = new List<ApplicationForm>();

            var application0 = new ApplicationForm()
            {
                Email = UsersGenerator.GetTestEmail("emily-davis", "anonym"),
                FullName = "Emily Davis",
                AdditionalInformation = 
                    "Dear Hiring Manager,\r\n\r\n" +
                    "I am writing to express my strong interest in the game development opportunities at your company. " +
                    "With a passion for creating immersive and engaging gaming experiences, I have spent the last five years honing my skills in game design and programming. " +
                    "My GitHub profile showcases my latest projects, and my LinkedIn profile offers more insights into my professional journey.\r\n\r\nI am excited to bring my creativity, " +
                    "problem-solving abilities, and technical expertise to your team. I am confident that my contributions can help elevate your game development projects to new heights. " +
                    "I look forward to the possibility of working together to create unforgettable gaming experiences." +
                    "\r\n\r\nSincerely,\r\nEmily Davis" +
                    "GitHub: https://github.com/scanax2\r\nLinkedIn: https://www.linkedin.com/in/daniel-parkhanovich-a31a54206/",
                AttachedResume = new AttachedResume() 
                {
                    File = GetEmployeeResume() 
                }
            };

            var application1 = new ApplicationForm()
            {
                Email = UsersGenerator.GetTestEmail("benjamin-hughes", "anonym"),
                FullName = "Benjamin Hughes",
                AttachedResume = new AttachedResume()
                {
                    File = GetEmployeeResume()
                }
            };

            var application2 = new ApplicationForm()
            {
                Email = UsersGenerator.GetTestEmail("william-turner", "anonym"),
                FullName = "William Turner",
                AttachedResume = new AttachedResume()
                {
                    File = GetEmployeeResume()
                }
            };

            applications.Add(application0);
            applications.Add(application1);
            applications.Add(application2);

            var users = await employeeManager.GetAllLoadedAsync();
            foreach (var user in users)
            {
                var userApplication = new ApplicationForm()
                {
                    Email = user.Email,
                    FullName = $"{user.Profile.Name} {user.Profile.Surname}",
                    AttachedResume = new AttachedResume()
                    {
                        ResumeUrl = user.Profile.ResumeUrl
                    },
                    AdditionalInformation = user.Profile.Description
                };
                applications.Add(userApplication);
            }

            var offersIds = await offerManager.GetAllIdsAsync();
            foreach (var offerId in offersIds)
            {
                foreach (var application in applications)
                {
                    application.OfferId = offerId;
                    await applicationsManager.PostApplicationFormAsync(offerId, null, application, null);
                }
            }
        }

        private ProfileImage GetCompanyImage(int index, string extension)
        {
            string directoryPath = Directory.GetCurrentDirectory();
            var formatted = string.Format(StaticFilesUtils.PathToExampleCompanyProfileImages, index, extension);

            string path = Path.Combine(directoryPath, formatted);

            return new ProfileImage()
            {
                File = StaticFilesUtils.GetFileAsFormFile(path, "image/bitmap")
            };
        }

        private ProfileImage GetEmployeeImage()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            return new ProfileImage()
            {
                File = StaticFilesUtils.GetFileAsFormFile(
                    Path.Combine(directoryPath, StaticFilesUtils.PathToExampleEmployeeProfileImages),
                    "image/bitmap")
            };
        }

        private IFormFile GetEmployeeResume()
        {
            string directoryPath = Directory.GetCurrentDirectory();
            return StaticFilesUtils.GetFileAsFormFile(
                Path.Combine(directoryPath, StaticFilesUtils.PathToExampleEmployeeResume),
                "application/pdf");
        }
    }
}

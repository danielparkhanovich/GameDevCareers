using JobBoardPlatform.BLL.Generators;
using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;

namespace JobBoardPlatform.BLL.Utils
{
    /// <summary>
    /// For tests only
    /// </summary>
    internal class EmployeesGenerator
    {
        private const string Password = "11111111";
        private readonly string[] AvailableYearsOfExperience = new string[] { "0-1", "1-2", "2-4", "4-8", "8+" };
        private readonly string[] Countries = new string[] { "Poland", "United States", "United Kingdom", "France", "Germany" };
        private readonly string[] Cities = new string[] { "Warsaw", "New York", "London", "Paris", "Berlin" };
        private readonly string ExampleUrl = "http://localhost:3000/";


        public IUserIdentityEntity GenerateData()
        {
            var newOfferData = new MockEmployeeData()
            {
                Email = $"{GetRandomHash()}@gmail.com",
                HashPassword = GetHashedPassword(),
                Profile = GenerateProfile(),
            };

            return newOfferData;
        }

        private EmployeeProfile GenerateProfile()
        {
            var random = new Random();
            (string city, string country) = GetRandomAddress(random);

            var profile = new EmployeeProfile()
            {
                Name = GetRandomHash(),
                Surname = GetRandomHash(),
                YearsOfExperience = GetRandomYears(random),
                ResumeUrl = ExampleUrl,
                Description = GetRandomHash(),
                City = city,
                Country = country,
                LinkedInUrl = ExampleUrl,
                ProfileImageUrl = string.Empty,
            };
            return profile;
        }

        private (string, string) GetRandomAddress(Random random)
        {
            int randomAddress = random.Next(Countries.Length);
            string country = Countries[randomAddress];
            string city = Cities[randomAddress];
            return (city, country);
        }

        private string GetRandomYears(Random random)
        {
            int randomIndex = random.Next(Countries.Length);
            return AvailableYearsOfExperience[randomIndex];
        }

        private string GetRandomHash()
        {
            var name = Guid.NewGuid();
            var hasher = new MD5Hasher();
            return hasher.GetHash(name.ToString());
        }

        private string GetHashedPassword()
        {
            var hasher = new MD5Hasher();
            return hasher.GetHash(Password);
        }
    }
}

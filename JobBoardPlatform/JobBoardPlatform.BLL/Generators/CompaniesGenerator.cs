using JobBoardPlatform.BLL.Generators;
using JobBoardPlatform.BLL.Services.Authentification;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Utils
{
    /// <summary>
    /// For tests only
    /// </summary>
    internal class CompaniesGenerator
    {
        private const string Password = "11111111";
        private readonly string[] Countries = new string[] { "Poland", "United States", "United Kingdom", "France", "Germany" };
        private readonly string[] Cities = new string[] { "Warsaw", "New York", "London", "Paris", "Berlin" };
        private readonly string CompanyWebsiteLink = "http://localhost:3000/";


        public IUserIdentityEntity GenerateData()
        {
            var newOfferData = new MockCompanyData()
            {
                Email = $"{GetRandomHash()}@gmail.com",
                HashPassword = GetHashedPassword(),
                Profile = GenerateProfile(),
            };

            return newOfferData;
        }

        private CompanyProfile GenerateProfile()
        {
            var random = new Random();
            (string city, string country) = GetRandomAddress(random);

            var profile = new CompanyProfile()
            {
                CompanyName = GetRandomHash(),
                City = city,
                Country = country,
                CompanyWebsiteUrl = CompanyWebsiteLink,
                ProfileImageUrl = string.Empty
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

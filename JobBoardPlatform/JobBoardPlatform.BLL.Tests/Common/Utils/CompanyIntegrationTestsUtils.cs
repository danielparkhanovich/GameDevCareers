using JobBoardPlatform.BLL.Boundaries;
using JobBoardPlatform.BLL.Commands.Identities;
using JobBoardPlatform.BLL.Commands.Identity;
using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Commands.Profile;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.IntegrationTests.Common.Mocks.DataStructures;
using JobBoardPlatform.IntegrationTests.Common.TestFiles;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Common.Utils
{
    internal class CompanyIntegrationTestsUtils
    {
        private readonly UserManager<CompanyIdentity> userManager;
        private readonly IOfferManager offersManager;
        private readonly IPasswordHasher passwordHasher;
        private readonly IRepository<CompanyIdentity> repository;
        private readonly IRepository<CompanyProfile> profileRepository;
        private readonly IRepository<JobOfferApplication> applicationRepository;
        private readonly IUserProfileImagesStorage imageStorage;
        // TODO: move anything related with offers to separate IntegrationUtils
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<JobOfferEmploymentDetails> offerDetailsRepository;
        private readonly IRepository<JobOfferSalariesRange> offerSalaryRangeRepository;


        public CompanyIntegrationTestsUtils(IServiceProvider serviceProvider)
        {
            userManager = serviceProvider.GetService<UserManager<CompanyIdentity>>()!;
            offersManager = serviceProvider.GetService<IOfferManager>()!;
            passwordHasher = serviceProvider.GetService<IPasswordHasher>()!;
            repository = serviceProvider.GetService<IRepository<CompanyIdentity>>()!;
            profileRepository = serviceProvider.GetService<IRepository<CompanyProfile>>()!;
            offersRepository = serviceProvider.GetService<IRepository<JobOffer>>()!;
            applicationRepository = serviceProvider.GetService<IRepository<JobOfferApplication>>()!;
            imageStorage = serviceProvider.GetService<IUserProfileImagesStorage>()!;
        }

        public string GetExampleCompanyEmail(int id = 0)
        {
            return $"company{id}@test.com";
        }

        public string GetExampleOfferTitle(int id = 0)
        {
            return $"test offer #{id}";
        }

        public async Task AddExampleCompaniesWithPublishedOffersAsync(int[] companyIds, int[] offersCount) 
        {
            for (int i = 0; i < companyIds.Length; i++)
            {
                await AddExampleCompanyWithPublishedOffersAsync(companyIds[i], offersCount[i]);
            }
        }

        public async Task AddExampleCompanyWithPublishedOffersAsync(int id, int offersCount)
        {
            string email = GetExampleCompanyEmail(id);
            await AddExampleCompanyAsync(email, "1234567890");

            for (int i = 0; i < offersCount; i++)
            {
                await AddPublishedOfferAsync(email, GetExampleOfferTitle(i));
            }
        }

        public Task AddExampleCompanyAsync(string email)
        {
            return AddExampleCompanyAsync(email, "1234567890");
        }

        public async Task AddExampleCompanyAsync(string email, string password)
        {
            string passwordHash = passwordHasher.GetHash(password);
            var employee = new CompanyIdentity()
            {
                Email = email,
                HashPassword = passwordHash,
                Profile = new CompanyProfile() 
                { 
                    CompanyName = "company",
                }
            };
            await userManager.AddNewUser(employee);
            await SetUserImageInProfile(email);
        }

        public async Task AddPublishedOfferAsync(string email, string offerName)
        {
            await AddNewOfferAsync(email, offerName);
            await PassPaymentAsync(email, offerName);
        }

        public async Task AddNewOfferAsync(string email, string offerName)
        {
            var user = await GetCompanyByEmail(email);
            await offersManager.AddAsync(user.ProfileId, GetExampleOfferData(offerName));
        }

        public async Task PassPaymentAsync(string email, string offerName)
        {
            var offer = await GetOfferAsync(email, offerName);
            await offersManager.PassPaymentAsync(offer!.Id);
        }

        public async Task CloseOffers(JobOffer[] offers)
        {
            foreach (var offer in offers)
            {
                await CloseOffer(offer.Id);
            }
        }

        public async Task CloseOffer(string email, string offerName)
        {
            var offer = await GetOfferAsync(email, offerName);
            await CloseOffer(offer!.Id);
        }

        public async Task CloseOffer(int offerId)
        {
            await offersManager.DeleteAsync(offerId);
        }

        public async Task DeleteCompany(string email)
        {
            var user = await userManager.GetUserByEmailAsync(email);
            var deleteCommand = new DeleteCompanyCommand(
                    repository,
                    profileRepository,
                    offersManager,
                    imageStorage,
                    user.Id);
            await deleteCommand.Execute();
        }

        public Task<CompanyIdentity> GetCompanyByEmail(string email)
        {
            return userManager.GetUserByEmailAsync(email);
        }

        public async Task<CompanyProfile> GetCompanyProfileByEmail(string email)
        {
            var user = await userManager.GetUserByEmailAsync(email);
            return await profileRepository.Get(user.ProfileId);
        }

        public async Task<CompanyProfile> GetCompanyProfileById(int profileId)
        {
            return await profileRepository.Get(profileId);
        }

        public async Task<JobOffer[]> GetOffersAsync(int[] companyIds, int[] offersCountToApply)
        {
            var offers = new List<JobOffer>();
            for (int i = 0; i < companyIds.Length; i++)
            {
                string company = GetExampleCompanyEmail(companyIds[i]);
                string offerTitle = GetExampleOfferTitle(i);
                offers.Add(await GetOfferAsync(company, offerTitle));
            }
            return offers.ToArray();
        }

        public Task<JobOffer> GetOfferAsync(int offerId)
        {
            return offersRepository.Get(offerId);
        }

        public Task<JobOffer> GetOfferDetailsAsync(int offerId)
        {
            return offersRepository.Get(offerId);
        }

        public async Task<JobOffer?> GetOfferAsync(string email, string offerTitle)
        {
            var user = await GetCompanyByEmail(email);
            return (await offersRepository.GetAll())
                .Where(x => x.CompanyProfileId == user.ProfileId && x.JobTitle == offerTitle)
                .SingleOrDefault();
        }

        public async Task<string[]> GetOfferAppliedResumeUrlsAsync(int offerId)
        {
            return (await applicationRepository.GetAll())
                .Where(x => x.JobOfferId == offerId)
                .Select(x => x.ResumeUrl)
                .ToArray();
        }

        public async Task<JobOfferApplication[]> GetOfferApplicationsAsync(int offerId)
        {
            return (await applicationRepository.GetAll()).Where(x => x.JobOfferId == offerId).ToArray();
        }

        public Task<bool> IsProfileImageInStorage(string url)
        {
            return imageStorage.IsImageExistsAsync(url);
        }

        public Task SetUserImageInProfile(string email)
        {
            var image = IntegrationTestFilesManager.GetCompanyProfileImageFile();
            var profileData = new CompanyProfileDataMock() 
            { 
                ProfileImage = new ProfileImageMock()
                {
                    File = image
                }
            };
            return UpdateCompanyProfile(email, profileData);
        }

        private async Task UpdateCompanyProfile(string email, ICompanyProfileData profileData)
        {
            var user = await GetCompanyByEmail(email);
            var updateCommand = new UpdateCompanyProfileCommand(
                user.Id,
                profileData,
                profileRepository,
                imageStorage);
            await updateCommand.Execute();
        }

        private OfferDataViewModel GetExampleOfferData(string offerName)
        {
            return new OfferDataViewModel()
            {
                JobTitle = offerName,
                ApplicationsContactType = 1,
                ApplicationsContactEmail = GetExampleCompanyEmail(),
                EmploymentTypes = GetExampleEmploymentTypes(),
                TechKeywords = new string[] { "C#", "OOP" },
                City = "test",
                Country = "test",
                JobDescription = "test"
            };
        }

        private EmploymentTypeViewModel[] GetExampleEmploymentTypes()
        {
            return new EmploymentTypeViewModel[2]
            {
                new EmploymentTypeViewModel()
                {
                    TypeId = 1,
                    SalaryFromRange = 7_000,
                    SalaryToRange = 9_000,
                    SalaryCurrencyType = 1,
                },
                new EmploymentTypeViewModel()
                {
                    TypeId = 3,
                    SalaryFromRange = 9_000,
                    SalaryToRange = 11_000,
                    SalaryCurrencyType = 1,
                }
            };
        }
    }
}

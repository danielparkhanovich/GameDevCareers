using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.IntegrationTests.Common.Assertions;
using JobBoardPlatform.IntegrationTests.Common.Fixtures;
using JobBoardPlatform.IntegrationTests.Common.Utils;

namespace JobBoardPlatform.IntegrationTests.Account.Delete
{
    public class DeleteCompanyTest : IClassFixture<AppFixture>, IDisposable
    {
        private readonly AppFixture fixture;
        private readonly CompanyIntegrationTestsUtils testsUtils;
        private readonly EmployeeIntegrationTestsUtils employeeUtils;
        private readonly CompanyProfileAssert assert;


        public DeleteCompanyTest(AppFixture fixture)
        {
            this.fixture = fixture;
            this.testsUtils = new CompanyIntegrationTestsUtils(fixture.ServiceProvider);
            this.employeeUtils = new EmployeeIntegrationTestsUtils(fixture.ServiceProvider);
            this.assert = new CompanyProfileAssert(
                testsUtils, 
                new ApplicationsIntegrationTestsUtils(fixture.ServiceProvider),
                new OfferIntegrationTestsUtils(fixture.ServiceProvider));
        }

        [Fact]
        public async Task DeleteNewAccountTest()
        {
            string userEmail = testsUtils.GetExampleCompanyEmail();
            await testsUtils.AddExampleCompanyAsync(userEmail);

            var userProfile = await testsUtils.GetCompanyProfileByEmail(userEmail);
            string attachedImageUrl = userProfile.ProfileImageUrl!;
            Assert.NotNull(userProfile);
            Assert.False(string.IsNullOrEmpty(attachedImageUrl));

            await testsUtils.DeleteCompany(userEmail);

            await assert.UserNotExists(userEmail, userProfile.Id);
            await assert.ProfileImageNotExists(attachedImageUrl);
        }

        [Fact]
        public async Task DeleteAccountWithNotPublishedOffersTest()
        {
            string userEmail = testsUtils.GetExampleCompanyEmail();
            var addedOffersIds = await AddCompanyWithOffersAsync(userEmail, 10, false);

            var userProfile = await testsUtils.GetCompanyProfileByEmail(userEmail);
            string attachedImageUrl = userProfile.ProfileImageUrl!;

            await testsUtils.DeleteCompany(userEmail);

            await assert.UserNotExists(userEmail, userProfile.Id);
            await assert.ProfileImageNotExists(attachedImageUrl);
            await assert.OffersAreClosed(addedOffersIds);
        }

        [Fact]
        public async Task DeleteAccountWithWithPublishedOffersTest()
        {
            string userEmail = testsUtils.GetExampleCompanyEmail();
            var addedOffersIds = await AddCompanyWithOffersAsync(userEmail, 10, true);

            var userProfile = await testsUtils.GetCompanyProfileByEmail(userEmail);
            string attachedImageUrl = userProfile.ProfileImageUrl!;

            await testsUtils.DeleteCompany(userEmail);

            await assert.UserNotExists(userEmail, userProfile.Id);
            await assert.ProfileImageNotExists(attachedImageUrl);
            await assert.OffersAreClosed(addedOffersIds);
        }

        [Fact]
        public async Task DeleteAccountWithWithPublishedAndAppliedOffersTest()
        {
            string userEmail = testsUtils.GetExampleCompanyEmail();
            var addedOffersIds = await AddCompanyWithOffersAsync(userEmail, 3, true);
            var userProfile = await testsUtils.GetCompanyProfileByEmail(userEmail);
            string attachedImageUrl = userProfile.ProfileImageUrl!;

            int registeredUsers = 5;
            await employeeUtils.AddExampleEmployeesAsync(registeredUsers);

            int registeredUsersWithAttachedResume = 2;
            await employeeUtils.SetUsersResumeInProfile(registeredUsersWithAttachedResume);

            await employeeUtils.ApplyUsersToOffer(0, addedOffersIds[0].Id);
            await employeeUtils.ApplyUsersToOffer(0, 3, addedOffersIds[1].Id);
            await employeeUtils.ApplyUsersToOffer(3, 10, addedOffersIds[2].Id);

            await testsUtils.DeleteCompany(userEmail);

            await assert.UserNotExists(userEmail, userProfile.Id);
            await assert.ProfileImageNotExists(attachedImageUrl);
            await assert.OffersAreClosed(addedOffersIds);
        }

        public void Dispose()
        {
            fixture.Dispose();
        }

        /// <returns>Added offers ids</returns>
        private async Task<List<JobOffer>> AddCompanyWithOffersAsync(string companyEmail, int offersCount, bool isPublish)
        {
            await testsUtils.AddExampleCompanyAsync(companyEmail);

            var addedOffersIds = new List<JobOffer>(offersCount);
            for (int i = 0; i < offersCount; i++)
            {
                string title = testsUtils.GetExampleOfferTitle(i);
                var offer = await AddOfferAsync(companyEmail, title, isPublish);
                addedOffersIds.Add(offer);
            }
            return addedOffersIds;
        }

        /// <returns>Added offer id</returns>
        private async Task<JobOffer> AddOfferAsync(string companyEmail, string title, bool isPublish)
        {
            if (isPublish)
            {
                await testsUtils.AddPublishedOfferAsync(companyEmail, title);
            }
            else
            {
                await testsUtils.AddNewOfferAsync(companyEmail, title);
            }
            return (await testsUtils.GetOfferAsync(companyEmail, title))!;
        }
    }
}
using JobBoardPlatform.IntegrationTests.Common.Assertions;
using JobBoardPlatform.IntegrationTests.Common.Fixtures;
using JobBoardPlatform.IntegrationTests.Common.Utils;

namespace JobBoardPlatform.IntegrationTests.Panel
{
    public class CompanyOffersPanelTest : IClassFixture<AppFixture>, IDisposable
    {
        private readonly AppFixture fixture;
        private readonly CompanyIntegrationTestsUtils testsUtils;
        private readonly EmployeeIntegrationTestsUtils employeeUtils;
        private readonly ApplicationsIntegrationTestsUtils applicationsUtils;
        private readonly CompanyProfileAssert assert;


        public CompanyOffersPanelTest(AppFixture fixture)
        {
            this.fixture = fixture;
            testsUtils = new CompanyIntegrationTestsUtils(fixture.ServiceProvider);
            employeeUtils = new EmployeeIntegrationTestsUtils(fixture.ServiceProvider);
            applicationsUtils = new ApplicationsIntegrationTestsUtils(fixture.ServiceProvider);
            assert = new CompanyProfileAssert(
                testsUtils, 
                applicationsUtils, 
                new OfferIntegrationTestsUtils(fixture.ServiceProvider));
        }

        [Fact]
        public async Task CreateOfferTest()
        {
            string email = testsUtils.GetExampleCompanyEmail();
            await testsUtils.AddExampleCompanyAsync(email);

            string offerTitle = testsUtils.GetExampleOfferTitle();
            await testsUtils.AddNewOfferAsync(email, offerTitle);

            await assert.OfferIsCreated(email, offerTitle);
        }

        [Fact]
        public async Task PublishOfferTest()
        {
            string email = testsUtils.GetExampleCompanyEmail();
            await testsUtils.AddExampleCompanyAsync(email);

            string offerTitle = testsUtils.GetExampleOfferTitle();
            await testsUtils.AddNewOfferAsync(email, offerTitle);
            await testsUtils.PassPaymentAsync(email, offerTitle);

            await assert.OfferIsPublished(email, offerTitle);
        }

        [Fact]
        public async Task CloseOfferTest()
        {
            string email = testsUtils.GetExampleCompanyEmail();
            string offerTitle = testsUtils.GetExampleOfferTitle();
            await testsUtils.AddExampleCompanyAsync(email);
            await testsUtils.AddPublishedOfferAsync(email, offerTitle);
            var offer = await testsUtils.GetOfferAsync(email, offerTitle);

            await testsUtils.CloseOffer(email, offerTitle);

            await assert.OfferIsClosed(offer!);
        }

        [Fact]
        public async Task CloseOfferWithApplicationsTest()
        {
            int registeredUsers = 15;
            int registeredUsersWithAttachedResume = 8;
            int totalApplied = 30;
            string companyEmail = testsUtils.GetExampleCompanyEmail();
            string offerTitle = testsUtils.GetExampleOfferTitle();
            await employeeUtils.AddExampleEmployeesAsync(registeredUsers);
            await employeeUtils.SetUsersResumeInProfile(registeredUsersWithAttachedResume);
            await testsUtils.AddExampleCompanyAsync(companyEmail);
            await testsUtils.AddPublishedOfferAsync(companyEmail, offerTitle);

            var offer = await testsUtils.GetOfferAsync(companyEmail, offerTitle);
            await employeeUtils.ApplyUsersToOffer(totalApplied, offer!.Id);
            var resumeUrls = await testsUtils.GetOfferAppliedResumeUrlsAsync(offer!.Id);
            await assert.ApplicationsAreAdded(offer!.Id, totalApplied, resumeUrls);
            await testsUtils.CloseOffer(companyEmail, offerTitle);

            await assert.OfferIsClosedAndResumesAreDeleted(offer, resumeUrls);
        }

        public void Dispose()
        {
            fixture.Dispose();
        }
    }
}
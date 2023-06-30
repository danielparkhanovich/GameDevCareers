using JobBoardPlatform.IntegrationTests.Common.Assertions;
using JobBoardPlatform.IntegrationTests.Common.Fixtures;
using JobBoardPlatform.IntegrationTests.Common.Utils;

namespace JobBoardPlatform.IntegrationTests.Profile
{
    public class EmployeeProfileResumeTest : IClassFixture<OffersManagementFixture>, IDisposable
    {
        private readonly OffersManagementFixture fixture;
        private readonly EmployeeIntegrationTestsUtils testsUtils;
        private readonly CompanyIntegrationTestsUtils companyUtils;
        private readonly EmployeeProfileAssert assert;


        public EmployeeProfileResumeTest(OffersManagementFixture fixture)
        {
            this.fixture = fixture;
            this.testsUtils = new EmployeeIntegrationTestsUtils(fixture.ServiceProvider);
            this.companyUtils = new CompanyIntegrationTestsUtils(fixture.ServiceProvider);
            this.assert = new EmployeeProfileAssert(testsUtils);
        }

        [Fact]
        public async Task AttachResumeToProfileTest()
        {
            string userEmail = testsUtils.GetUserExampleEmail();
            await testsUtils.AddExampleEmployeeAsync(userEmail);

            await testsUtils.SetUserResumeInProfile(userEmail);
            string attachedResumeUrl = await testsUtils.GetResumeUrl(userEmail);

            await assert.ResumeExists(attachedResumeUrl);
        }

        [Fact]
        public async Task DeleteEmployeeResumeFromProfileTest()
        {
            string userEmail = testsUtils.GetUserExampleEmail();
            await testsUtils.AddExampleEmployeeAsync(userEmail);
            await testsUtils.SetUserResumeInProfile(userEmail);
            string attachedResumeUrl = await testsUtils.GetResumeUrl(userEmail);

            await testsUtils.TryDeleteEmployeeProfileResume(userEmail);

            await assert.ResumeNotExists(attachedResumeUrl);
        }

        [Fact]
        public async Task DeleteEmployeeResumeAppliedForOffersFromProfileTest()
        {
            int[] companyIds = new int[] { 0, 1, 2 };
            int[] offersCount = new int[] { 1, 2, 3 };
            await companyUtils.AddExampleCompaniesWithPublishedOffersAsync(companyIds, offersCount);

            string userEmail = testsUtils.GetUserExampleEmail();
            await testsUtils.AddExampleEmployeeAsync(userEmail);
            await testsUtils.SetUserResumeInProfile(userEmail);
            string attachedResumeUrl = await testsUtils.GetResumeUrl(userEmail);

            var offers = await companyUtils.GetOffersAsync(companyIds, offersCount);
            await testsUtils.ApplyToOffers(userEmail, offers);

            await testsUtils.TryDeleteEmployeeProfileResume(userEmail);

            await assert.ResumeExists(attachedResumeUrl);
        }

        [Fact]
        public async Task DeleteEmployeeResumeAppliedForClosedOffersFromProfileTest()
        {
            int[] companyIds = new int[] { 0, 1, 2 };
            int[] offersCount = new int[] { 1, 2, 3 };
            await companyUtils.AddExampleCompaniesWithPublishedOffersAsync(companyIds, offersCount);

            string userEmail = testsUtils.GetUserExampleEmail();
            await testsUtils.AddExampleEmployeeAsync(userEmail);
            await testsUtils.SetUserResumeInProfile(userEmail);
            string attachedResumeUrl = await testsUtils.GetResumeUrl(userEmail);

            var offers = await companyUtils.GetOffersAsync(companyIds, offersCount);
            await testsUtils.ApplyToOffers(userEmail, offers);

            await companyUtils.CloseOffers(offers);
            await assert.ResumeExists(attachedResumeUrl);
            await testsUtils.TryDeleteEmployeeProfileResume(userEmail);

            await assert.ResumeNotExists(attachedResumeUrl);
        }

        public void Dispose()
        {
            fixture.Dispose();
        }
    }
}
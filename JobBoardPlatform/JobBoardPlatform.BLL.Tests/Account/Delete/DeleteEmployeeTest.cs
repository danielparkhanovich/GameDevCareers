using JobBoardPlatform.IntegrationTests.Common.Assertions;
using JobBoardPlatform.IntegrationTests.Common.Fixtures;
using JobBoardPlatform.IntegrationTests.Common.Utils;

namespace JobBoardPlatform.IntegrationTests.Account.Delete
{
    public class DeleteEmployeeTest : IClassFixture<AppFixture>, IDisposable
    {
        private readonly AppFixture fixture;
        private readonly EmployeeIntegrationTestsUtils testsUtils;
        private readonly CompanyIntegrationTestsUtils companyUtils;
        private readonly EmployeeProfileAssert assert;


        public DeleteEmployeeTest(AppFixture fixture)
        {
            this.fixture = fixture;
            testsUtils = new EmployeeIntegrationTestsUtils(fixture.ServiceProvider);
            companyUtils = new CompanyIntegrationTestsUtils(fixture.ServiceProvider);
            assert = new EmployeeProfileAssert(testsUtils);
        }

        [Fact]
        public async Task DeleteNewAccountTest()
        {
            string userEmail = testsUtils.GetUserExampleEmail();
            await testsUtils.AddExampleEmployeeAsync(userEmail);
            var addedUserProfile = await testsUtils.GetEmployeeProfileByEmail(userEmail);
            Assert.NotNull(addedUserProfile);

            await testsUtils.DeleteEmployee(userEmail);

            await assert.UserNotExists(userEmail, addedUserProfile.Id);
        }

        [Fact]
        public async Task DeleteAccountWithResumeTest()
        {
            string userEmail = testsUtils.GetUserExampleEmail();
            await testsUtils.AddExampleEmployeeAsync(userEmail);
            await testsUtils.SetUserResumeInProfile(userEmail);
            var userProfile = await testsUtils.GetEmployeeProfileByEmail(userEmail);

            string attachedResumeUrl = userProfile.ResumeUrl!;
            Assert.False(string.IsNullOrEmpty(attachedResumeUrl));

            await testsUtils.DeleteEmployee(userEmail);

            await assert.UserNotExists(userEmail, userProfile.Id);
            await assert.ResumeNotExists(attachedResumeUrl);
        }

        [Fact]
        public async Task DeleteAccountWithProfileImageTest()
        {
            string userEmail = testsUtils.GetUserExampleEmail();
            await testsUtils.AddExampleEmployeeAsync(userEmail);
            await testsUtils.SetUserImageInProfile(userEmail);
            var userProfile = await testsUtils.GetEmployeeProfileByEmail(userEmail);

            string attachedImageUrl = userProfile.ProfileImageUrl!;
            Assert.False(string.IsNullOrEmpty(attachedImageUrl));

            await testsUtils.DeleteEmployee(userEmail);

            await assert.UserNotExists(userEmail, userProfile.Id);
            await assert.ProfileImageNotExists(attachedImageUrl);
        }

        [Fact]
        public async Task DeleteAccountWithAllDataAndAppliedToOffersTest()
        {
            int[] companyIds = new int[] { 0, 1, 2 };
            int[] offersCount = new int[] { 1, 2, 3 };
            await companyUtils.AddExampleCompaniesWithPublishedOffersAsync(companyIds, offersCount);

            string userEmail = testsUtils.GetUserExampleEmail();
            await testsUtils.AddExampleEmployeeAsync(userEmail);
            await testsUtils.SetUserImageInProfile(userEmail);
            await testsUtils.SetUserResumeInProfile(userEmail);

            var offers = await companyUtils.GetOffersAsync(companyIds);
            await testsUtils.ApplyToOffers(userEmail, offers);

            string attachedResumeUrl = await testsUtils.GetResumeUrl(userEmail);
            int profileId = (await testsUtils.GetEmployeeProfileByEmail(userEmail)).Id;
            await testsUtils.DeleteEmployee(userEmail);

            await assert.UserNotExists(userEmail, profileId);
            await assert.ProfileImageNotExists(userEmail);
            await assert.ResumeExists(attachedResumeUrl);
        }

        public void Dispose()
        {
            fixture.Dispose();
        }
    }
}
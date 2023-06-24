using JobBoardPlatform.BLL.IntegrationTests.Fixtures;
using JobBoardPlatform.IntegrationTests.Utils;

namespace JobBoardPlatform.BLL.IntegrationTests.Commands
{
    public class DeleteEmployeeCommandTest : IClassFixture<DataManagementFixture>, IDisposable
    {
        private readonly DataManagementFixture fixture;
        private readonly RepositoryIntegrationTestsUtils testsUtils;
        private readonly RepositoryAssert assert;


        public DeleteEmployeeCommandTest(DataManagementFixture fixture)
        {
            this.fixture = fixture;
            this.testsUtils = new RepositoryIntegrationTestsUtils(fixture.ServiceProvider);
            this.assert = new RepositoryAssert(testsUtils);
        }

        [Fact]
        public async Task TestDeleteNewAccount()
        {
            string userEmail = IntegrationTestsGlobals.ExampleEmail;
            await testsUtils.AddExampleEmployeeToRepositoryAsync(userEmail);
            var addedUserProfile = await testsUtils.GetEmployeeProfileByEmail(userEmail);
            Assert.NotNull(addedUserProfile);

            var deleteCommand = await testsUtils.GetDeleteEmployeeCommandByEmail(userEmail);
            await deleteCommand.Execute();

            await assert.UserNotExists(userEmail, addedUserProfile.Id);
        }

        [Fact]
        public async Task TestDeleteAccountWithResume()
        {
            string userEmail = IntegrationTestsGlobals.ExampleEmail;
            await testsUtils.AddExampleEmployeeToRepositoryAsync(userEmail);
            await testsUtils.SetUserResumeInProfile(userEmail);
            var userProfile = await testsUtils.GetEmployeeProfileByEmail(userEmail);
            string attachedResumeUrl = userProfile.ResumeUrl!;
            Assert.False(string.IsNullOrEmpty(attachedResumeUrl));

            var deleteCommand = await testsUtils.GetDeleteEmployeeCommandByEmail(userEmail);
            await deleteCommand.Execute();
            
            await assert.UserNotExists(userEmail, userProfile.Id);
            await assert.ResumeNotExists(userEmail);
        }

        [Fact]
        public async Task TestDeleteAccountWithProfileImage()
        {
            string userEmail = IntegrationTestsGlobals.ExampleEmail;
            await testsUtils.AddExampleEmployeeToRepositoryAsync(userEmail);
            await testsUtils.SetUserImageInProfile(userEmail);
            var userProfile = await testsUtils.GetEmployeeProfileByEmail(userEmail);
            string attachedImageUrl = userProfile.ProfileImageUrl!;
            Assert.False(string.IsNullOrEmpty(attachedImageUrl));

            var deleteCommand = await testsUtils.GetDeleteEmployeeCommandByEmail(userEmail);
            await deleteCommand.Execute();

            await assert.UserNotExists(userEmail, userProfile.Id);
            await assert.ProfileImageNotExists(userEmail);
        }

        [Fact]
        public async Task TestDeleteAccountWithAllDataAndAppliedToOffers()
        {
            string userEmail = IntegrationTestsGlobals.ExampleEmail;
            await testsUtils.AddExampleEmployeeToRepositoryAsync(userEmail);
            await testsUtils.SetUserImageInProfile(userEmail);
            await testsUtils.SetUserResumeInProfile(userEmail);
            await testsUtils.ApplyToOffer(userEmail, 0);
            await testsUtils.ApplyToOffer(userEmail, 0);
            await testsUtils.ApplyToOffer(userEmail, 0);
            await testsUtils.ApplyToOffer(userEmail, 0);
            var userProfile = await testsUtils.GetEmployeeProfileByEmail(userEmail);

            var deleteCommand = await testsUtils.GetDeleteEmployeeCommandByEmail(userEmail);
            await deleteCommand.Execute();

            await assert.UserNotExists(userEmail, userProfile.Id);
            await assert.ProfileImageNotExists(userEmail);
            await assert.ResumeNotExists(userEmail);
        }

        public void Dispose()
        {
            fixture.Dispose();
        }
    }
}
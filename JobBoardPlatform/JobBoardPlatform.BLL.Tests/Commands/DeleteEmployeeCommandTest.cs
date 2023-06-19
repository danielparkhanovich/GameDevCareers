using JobBoardPlatform.BLL.IntegrationTests.Fixtures;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using JobBoardPlatform.IntegrationTests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.BLL.IntegrationTests.Commands
{
    public class DeleteEmployeeCommandTest : IClassFixture<DataManagementFixture>
    {
        private readonly IServiceProvider serviceProvider;
        private readonly RepositoryIntegrationTestsUtils dataManagementTestUtils;
        private readonly IBlobStorage profileImagesStorage;
        private readonly IBlobStorage profileResumesStorage;
        private readonly IBlobStorage applicationResumesStorage;


        public DeleteEmployeeCommandTest(DataManagementFixture fixture)
        {
            this.serviceProvider = fixture.ServiceProvider;
            this.dataManagementTestUtils = new RepositoryIntegrationTestsUtils(serviceProvider);
            this.profileImagesStorage = serviceProvider.GetService<UserProfileImagesStorage>()!;
            this.profileResumesStorage = serviceProvider.GetService<UserProfileAttachedResumeStorage>()!;
            this.applicationResumesStorage = serviceProvider.GetService<UserApplicationsResumeStorage>()!;
        }

        [Fact]
        public async Task TestDeleteNewAccount()
        {
            string userEmail = "test@mail.com";
            await dataManagementTestUtils.AddExampleEmployeeToRepositoryAsync(userEmail);

            var deleteCommand = await dataManagementTestUtils.GetDeleteEmployeeCommandByEmail(userEmail);
            await deleteCommand.Execute();

            var user = await dataManagementTestUtils.GetEmployeeByEmail(userEmail);
            Assert.True(user == null, "Employee repository must be empty!");
        }

        [Fact]
        public async Task TestDeleteAccountWithResume()
        {
            string userEmail = "test@mail.com";
            await dataManagementTestUtils.AddExampleEmployeeToRepositoryAsync(userEmail);



            var deleteCommand = await dataManagementTestUtils.GetDeleteEmployeeCommandByEmail(userEmail);
            await deleteCommand.Execute();

            var metadata = await context.GetBlobMetadataAsync(url);
            Assert.Equal(metadata.Name, string.Empty);
            Assert.Equal(metadata.Size, string.Empty);
        }

        [Fact]
        public void TestDeleteAccountWithProfileImage()
        {

        }

        [Fact]
        public void TestDeleteAccountWithProfileImageAndResume()
        {

        }

        [Fact]
        public void TestDeleteAccountWithAllDataAndAppliedToOffers()
        {

        }
    }
}
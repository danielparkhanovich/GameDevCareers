
namespace JobBoardPlatform.IntegrationTests.Utils
{
    internal class RepositoryAssert
    {
        private readonly RepositoryIntegrationTestsUtils testsUtils;


        public RepositoryAssert(RepositoryIntegrationTestsUtils testsUtils)
        {
            this.testsUtils = testsUtils;
        }

        public async Task ResumeExists(string resumeUrl)
        {
            Assert.True(await testsUtils.IsResumeInStorage(resumeUrl));
        }

        public async Task ResumeNotExists(string resumeUrl)
        {
            Assert.False(await testsUtils.IsResumeInStorage(resumeUrl));

            var metadata = await testsUtils.GetResumeMetadataFromStorage(resumeUrl);
            Assert.True(string.IsNullOrEmpty(metadata.Name));
            Assert.True(string.IsNullOrEmpty(metadata.Size));
        }

        public async Task ProfileImageExists(string imageUrl)
        {
            Assert.True(await testsUtils.IsProfileImageInStorage(imageUrl));
        }

        public async Task ProfileImageNotExists(string imageUrl)
        {
            Assert.False(await testsUtils.IsProfileImageInStorage(imageUrl));
        }

        public async Task UserNotExists(string userEmail, int userProfileId)
        {
            var user = await testsUtils.GetEmployeeByEmail(userEmail);
            var userProfile = await testsUtils.GetEmployeeProfileById(userProfileId);

            Assert.Null(user);
            Assert.Null(userProfile);
        }
    }
}

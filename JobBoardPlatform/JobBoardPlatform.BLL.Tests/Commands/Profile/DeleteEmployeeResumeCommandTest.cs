using JobBoardPlatform.BLL.IntegrationTests.Fixtures;
using JobBoardPlatform.IntegrationTests.Utils;

namespace JobBoardPlatform.IntegrationTests.Commands.Profile
{
    public class DeleteEmployeeResumeCommandTest : IClassFixture<DataManagementFixture>, IDisposable
    {
        private readonly DataManagementFixture fixture;
        private readonly RepositoryIntegrationTestsUtils testsUtils;
        private readonly RepositoryAssert assert;


        public DeleteEmployeeResumeCommandTest(DataManagementFixture fixture)
        {
            this.fixture = fixture;
            testsUtils = new RepositoryIntegrationTestsUtils(fixture.ServiceProvider);
            assert = new RepositoryAssert(testsUtils);
        }

        [Fact]
        public async Task Test()
        {
            
        }

        public void Dispose()
        {
            fixture.Dispose();
        }
    }
}
using JobBoardPlatform.IntegrationTests.Common.Assertions;
using JobBoardPlatform.IntegrationTests.Common.Fixtures;
using JobBoardPlatform.IntegrationTests.Common.Utils;

namespace JobBoardPlatform.IntegrationTests.Account.Delete
{
    public class DeleteCompanyTest : IClassFixture<CompanyAccountManagementFixture>, IDisposable
    {
        private readonly CompanyAccountManagementFixture fixture;
        private readonly CompanyIntegrationTestsUtils testsUtils;
        private readonly EmployeeProfileAssert assert;


        public DeleteCompanyTest(CompanyAccountManagementFixture fixture)
        {
            this.fixture = fixture;
            testsUtils = new CompanyIntegrationTestsUtils(fixture.ServiceProvider);
            // this.assert = new UserProfileAssert(testsUtils);
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
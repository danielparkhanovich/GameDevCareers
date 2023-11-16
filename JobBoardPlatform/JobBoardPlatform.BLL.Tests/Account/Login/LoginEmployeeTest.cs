using JobBoardPlatform.IntegrationTests.Common.Assertions;
using JobBoardPlatform.IntegrationTests.Common.Fixtures;
using JobBoardPlatform.IntegrationTests.Common.Utils;

namespace JobBoardPlatform.IntegrationTests.Account.Delete
{
    public class LoginEmployeeTest : IClassFixture<AppFixture>, IDisposable
    {
        private readonly AppFixture fixture;
        private readonly EmployeeIntegrationTestsUtils employeeUtils;
        private readonly EmployeeProfileAssert assert;


        public LoginEmployeeTest(AppFixture fixture)
        {
            this.fixture = fixture;
            this.employeeUtils = new EmployeeIntegrationTestsUtils(fixture.ServiceProvider);
            this.assert = new EmployeeProfileAssert(employeeUtils);
        }

        [Fact]
        public async Task InvalidEmailTest()
        {

        }

        [Fact]
        public async Task InvalidPasswordTest()
        {
            
        }

        [Fact]
        public async Task LoginWithProviderTest()
        {

        }

        [Fact]
        public async Task CorrectLoginTest()
        {

        }

        public void Dispose()
        {
            fixture.Dispose();
        }
    }
}
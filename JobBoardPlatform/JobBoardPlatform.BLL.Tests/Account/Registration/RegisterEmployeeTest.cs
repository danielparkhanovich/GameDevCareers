using JobBoardPlatform.IntegrationTests.Common.Fixtures;

namespace JobBoardPlatform.IntegrationTests.Account.Registration
{
    public class RegisterEmployeeTest : IClassFixture<AppFixture>, IDisposable
    {
        public RegisterEmployeeTest()
        {

        }

        [Fact]
        public Task Test()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
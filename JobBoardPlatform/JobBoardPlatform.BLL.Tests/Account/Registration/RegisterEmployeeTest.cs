using JobBoardPlatform.IntegrationTests.Common.Fixtures;

namespace JobBoardPlatform.IntegrationTests.Account.Registration
{
    public class RegisterEmployeeTest : IClassFixture<OffersManagementFixture>, IDisposable
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
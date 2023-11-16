using Microsoft.Extensions.DependencyInjection;
using JobBoardPlatform.IntegrationTests.Common.Mocks.Services;

namespace JobBoardPlatform.IntegrationTests.Common.Fixtures
{
    public class DbFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; protected set; }
        protected readonly ServiceCollection services;


        public DbFixture()
        {
            services = new ServiceCollection();
            TestSetup.AddSqlRepository(services);
            TestSetup.AddAzureStorages(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            TestSetup.DisposeAllData(ServiceProvider);
        }
    }
}

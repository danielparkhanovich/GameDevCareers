using JobBoardPlatform.DAL.Data;
using JobBoardPlatform.DAL.Repositories.Blob;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.IntegrationTests.Common.Mocks.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Common.Fixtures
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IDisposable
        where TProgram : class
    {
        private IServiceProvider serviceProvider;


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var repositoryDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DataContext));
                services.Remove(repositoryDescriptor!);

                var storageDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(CoreBlobStorage));
                services.Remove(storageDescriptor!);

                TestSetup.AddSqlRepository(services);
                TestSetup.AddAzureStorages(services);
                TestSetup.AddMockServices(services);
                serviceProvider = services.BuildServiceProvider();
            });

            builder.UseEnvironment("Test");
        }

        protected override void Dispose(bool disposing)
        {
            //base.Dispose(disposing);
            //TestSetup.DisposeAllData(serviceProvider);
        } 
    }
}

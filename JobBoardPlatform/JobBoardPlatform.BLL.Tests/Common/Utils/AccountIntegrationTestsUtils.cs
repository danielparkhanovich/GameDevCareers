using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Common.Utils
{
    internal class AccountIntegrationTestsUtils
    {
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage applicationsResumeStorage;


        public AccountIntegrationTestsUtils(IServiceProvider serviceProvider)
        {
            profileResumeStorage = serviceProvider.GetService<IProfileResumeBlobStorage>()!;
            applicationsResumeStorage = serviceProvider.GetService<IApplicationsResumeBlobStorage>()!;
        }
    }
}

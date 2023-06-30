using JobBoardPlatform.DAL.Repositories.Blob.AttachedResume;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Common.Utils
{
    internal class ApplicationsIntegrationTestsUtils
    {
        private readonly IProfileResumeBlobStorage profileResumeStorage;
        private readonly IApplicationsResumeBlobStorage applicationsResumeStorage;


        public ApplicationsIntegrationTestsUtils(IServiceProvider serviceProvider)
        {
            profileResumeStorage = serviceProvider.GetService<IProfileResumeBlobStorage>()!;
            applicationsResumeStorage = serviceProvider.GetService<IApplicationsResumeBlobStorage>()!;
        }

        public async Task<bool> IsResumesInStorage(string[] urls)
        {
            foreach (var url in urls)
            {
                if(!await IsResumeInStorage(url))
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> IsResumeInStorage(string url)
        {
            bool isInApplicationsResumes = await applicationsResumeStorage.IsExistsAsync(url);
            bool isInProfileResumes = await profileResumeStorage.IsExistsAsync(url);
            return isInApplicationsResumes || isInProfileResumes;
        }
    }
}

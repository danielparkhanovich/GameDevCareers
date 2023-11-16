using JobBoardPlatform.BLL.DTOs;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.Services
{
    internal class EmailViewRendererMock : IEmailContent<JobOfferApplication>
    {
        public Task<string> GetMessageAsync(JobOfferApplication value)
        {
            return Task.FromResult("Test message");
        }

        public Task<string> GetSubjectAsync(JobOfferApplication value)
        {
            return Task.FromResult("Test subject");
        }
    }
}

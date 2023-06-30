using JobBoardPlatform.BLL.Commands.Contracts;

namespace JobBoardPlatform.IntegrationTests.Common.Mocks.DataStructures
{
    public class ApplicationFormMock : IApplicationForm
    {
        public int OfferId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public IAttachedResume AttachedResume { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}

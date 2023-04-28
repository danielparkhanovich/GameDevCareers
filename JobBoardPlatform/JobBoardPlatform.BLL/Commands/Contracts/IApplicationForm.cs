namespace JobBoardPlatform.BLL.Commands.Contracts
{
    public interface IApplicationForm
    {
        public int OfferId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public IAttachedResume AttachedResume { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}

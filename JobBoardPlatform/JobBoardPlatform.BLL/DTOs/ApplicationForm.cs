namespace JobBoardPlatform.BLL.DTOs
{
    public class ApplicationForm
    {
        public int OfferId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public AttachedResume AttachedResume { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}

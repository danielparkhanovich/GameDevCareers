namespace JobBoardPlatform.PL.ViewModels.Profile.Contracts
{
    /// <summary>
    /// Fields allowed for direct changes from input field
    /// </summary>
    public interface ICompanyProfileSharedFields
    {
        public string? CompanyName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? CompanyWebsiteUrl { get; set; }
    }
}

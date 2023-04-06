namespace JobBoardPlatform.PL.ViewModels.Profile.Contracts
{
    /// <summary>
    /// Fields allowed for direct changes from input field
    /// </summary>
    public interface IEmployeeProfileSharedFields
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        public string? YearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
    }
}

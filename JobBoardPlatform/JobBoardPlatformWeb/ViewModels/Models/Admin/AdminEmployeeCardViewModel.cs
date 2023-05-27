using JobBoardPlatform.PL.ViewModels.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Admin
{
    public class AdminEmployeeCardViewModel : IContainerCard
    {
        public string PartialView => "./Admin/_EmployeeCardAdminView";
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Surname { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; } = string.Empty;
        public string? AttachedResumeUrl { get; set; } = string.Empty;
        public string? YearsOfExperience { get; set; } = string.Empty;
        public string? LinkedInUrl { get; set; } = string.Empty;
    }
}

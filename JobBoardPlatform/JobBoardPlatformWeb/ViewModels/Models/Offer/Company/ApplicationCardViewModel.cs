using JobBoardPlatform.PL.ViewModels.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Company
{
    public class ApplicationCardViewModel : IContainerCard
    {
        public string PartialView => "./JobOffers/_ApplicationCard";
        public string EmailView => "./Email/_ApplicationForm";


        public int Id { get; set; }
        public int OfferId { get; set; }
        public int PriorityFlagId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
        public string ApplicatedAgo { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? GitHubUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? YearsOfExperience { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Description { get; set; }
        public int? CoverLetterWordsCount { get; set; }
    }
}

using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.DAL.Models
{
    public class CompanyProfile : IEntity
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string ResumeUrl { get; set; } = string.Empty;
    }
}

using JobBoardPlatform.BLL.Boundaries;

namespace JobBoardPlatform.PL.ViewModels.Models.Profile.Common
{
    public class ProfileImageViewModel : IProfileImage
    {
        public IFormFile? File { get; set; }
        public string? ImageUrl { get; set; }
    }
}

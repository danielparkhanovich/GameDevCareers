namespace JobBoardPlatform.PL.ViewModels.Models.Profile.Contracts
{
    public interface IProfileViewModel
    {
        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImageUrl { get; }
    }
}

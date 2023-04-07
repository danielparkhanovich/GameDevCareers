namespace JobBoardPlatform.PL.ViewModels.Profile.Contracts
{
    public interface IProfileViewModel
    {
        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImageUrl { get; }
    }
}

namespace JobBoardPlatform.PL.ViewModels.Profile.Contracts
{
    public interface IProfileViewModel
    {
        public IFormFile? Avatar { get; set; }
        public string PhotoUrl { get; set; }
    }
}

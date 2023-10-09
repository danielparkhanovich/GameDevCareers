namespace JobBoardPlatform.BLL.Boundaries
{
    public interface ILoginSettingsData
    {
        public string? Login { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}

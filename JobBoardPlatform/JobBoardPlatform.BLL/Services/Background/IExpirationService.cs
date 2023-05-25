namespace JobBoardPlatform.BLL.Services.Background
{
    internal interface IExpirationService
    {
        public bool IsExpired();
        public int GetDaysLeft();
    }
}

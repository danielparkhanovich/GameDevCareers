namespace JobBoardPlatform.BLL.Utilities
{
    public class AuthorizationResult
    {
        public string? Error { get; set; }
        public bool IsError { get => Error != null; }
    }
}

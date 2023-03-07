namespace JobBoardPlatform.BLL.Utilities
{
    public class AuthorizationResult
    {
        public string? Error { get; set; }
        public bool IsError { get => Error != null; }


        public static AuthorizationResult Success => new AuthorizationResult();
    }
}

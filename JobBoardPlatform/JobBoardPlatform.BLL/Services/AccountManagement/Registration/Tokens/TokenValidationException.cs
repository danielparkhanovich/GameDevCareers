
namespace JobBoardPlatform.BLL.Services.AccountManagement.Registration.Tokens
{
    public class TokenValidationException : Exception
    {
        public const string WrongToken = "Token not found";
        

        public TokenValidationException()
        {
        }

        public TokenValidationException(string message)
            : base(message)
        {
        }

        public TokenValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

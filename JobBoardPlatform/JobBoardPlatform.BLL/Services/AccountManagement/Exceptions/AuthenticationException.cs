
namespace JobBoardPlatform.BLL.Services.Authentification.Exceptions
{
    public class AuthenticationException : Exception
    {
        public const string EmailNotFound = "Email is not found";
        public const string EmailAlreadyRegistered = "Email is already registered";
        public const string WrongEmail = "Wrong email";
        public const string WrongPassword = "Wrong password";


        public AuthenticationException()
        {
        }

        public AuthenticationException(string message)
            : base(message)
        {
        }

        public AuthenticationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

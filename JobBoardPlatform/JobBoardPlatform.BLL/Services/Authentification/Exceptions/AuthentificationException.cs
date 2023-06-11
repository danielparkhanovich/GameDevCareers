
namespace JobBoardPlatform.BLL.Services.Authentification.Exceptions
{
    public class AuthentificationException : Exception
    {
        public const string EmailNotFound = "Email is not found";
        public const string EmailAlreadyRegistered = "Email is already registered";
        public const string EmailWrong = "Email is wrong";
        public const string WrongPassword = "Wrong password";


        public AuthentificationException()
        {
        }

        public AuthentificationException(string message)
            : base(message)
        {
        }

        public AuthentificationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using JobBoardPlatform.BLL.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    internal class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            string hashText;
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Unicode.GetBytes("TextToHash"));
                StringBuilder hashTextBuilder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    hashTextBuilder.Append(hash[i].ToString("x2"));
                }
                hashText = hashTextBuilder.ToString();
            }
            return hashText;
        }

        public AuthorizationResult VerifyHashedPassword(string providedPassword, string hashedPassword)
        {
            var result = new AuthorizationResult();

            var hashedProvided = HashPassword(hashedPassword);
            if (hashedProvided != hashedPassword)
            {
                result.Error = "Password is false";
            }

            return result;
        }
    }
}

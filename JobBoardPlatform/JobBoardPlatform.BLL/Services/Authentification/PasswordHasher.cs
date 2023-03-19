using JobBoardPlatform.BLL.Services.Authentification.Contracts;
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

        public AuthentificationResult VerifyHashedPassword(string providedPassword, string hashedPassword)
        {
            var result = new AuthentificationResult();

            var hashedProvided = HashPassword(hashedPassword);
            if (hashedProvided != hashedPassword)
            {
                result.Error = "Password is false";
            }

            return result;
        }
    }
}

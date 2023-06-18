using System.Security.Cryptography;
using JobBoardPlatform.BLL.Services.Authentification.Contracts;

namespace JobBoardPlatform.BLL.Services.AccountManagement.Common
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private const int PasswordLengthInSymbols = 8;
        private const string PasswordAvailableTokens =
            "abcdefghijklmnopqrstuvwxyz" +
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "1234567890" +
            "!@#$%&*?";


        public string GeneratePassword()
        {
            int length = PasswordLengthInSymbols;

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);
                char[] chars = new char[length];

                for (int i = 0; i < length; i++)
                {
                    int index = randomBytes[i] % PasswordAvailableTokens.Length;
                    chars[i] = PasswordAvailableTokens[index];
                }

                return new string(chars);
            }
        }
    }
}

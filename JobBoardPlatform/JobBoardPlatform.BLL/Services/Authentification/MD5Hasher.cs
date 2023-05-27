using JobBoardPlatform.BLL.Services.Authentification.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace JobBoardPlatform.BLL.Services.Authentification
{
    internal class MD5Hasher : IPasswordHasher
    {
        public string GetHash(string password)
        {
            string hashText;
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Unicode.GetBytes(password));
                StringBuilder hashTextBuilder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    hashTextBuilder.Append(hash[i].ToString("x2"));
                }
                hashText = hashTextBuilder.ToString();
            }
            return hashText;
        }
    }
}

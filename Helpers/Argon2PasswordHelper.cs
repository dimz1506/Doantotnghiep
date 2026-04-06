using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace Doantotnghiep.Helpers
{
    public static class Argon2PasswordHelper
    {
        // tao salt ngau nhien
        public static string GenerateSalt(int size = 16)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(size);
            return Convert.ToBase64String(saltBytes);
        }
        // hash password voi salt
        public static async Task<string> HashPasswordAsync(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = saltBytes,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 1024 * 64
            };
            var hashBytes = await argon2.GetBytesAsync(16);
            return Convert.ToBase64String(hashBytes);
        }
        // kiem tra password
        public static async Task<bool> VerifyPasswordAsync(string password, string salt, string hash)
        {
            var newHash = await HashPasswordAsync(password, salt);
            return newHash == hash;
        }
    }
}

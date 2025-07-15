using MF2024_API.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MF2024_API.Service
{
    public class AESService : AESInterfaces
    {
        private static readonly byte[] key = Encoding.UTF8.GetBytes("16CharacterKey!!");
        private static readonly byte[] iv = Encoding.UTF8.GetBytes("16CharacterIV!!!");


        public async Task<string> Decrypt(string text)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;


                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(text);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        public async Task< string> Encrypt(string text)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(text);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }
    }
}

namespace MF2024_API.Interfaces
{
    public interface AESInterfaces
    {
        Task<string> Encrypt(string text);
        Task<string> Decrypt(string text);
    }
}

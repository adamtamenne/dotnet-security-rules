using System.Security.Cryptography;
using System.Text;

public class WeakCryptoSafe
{
    // ok: dotnet-weak-crypto-md5 / dotnet-weak-crypto-sha1
    // SHA-256 is acceptable for integrity checks
    public byte[] HashIntegritySha256(string data)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    // ok: dotnet-weak-crypto-md5 / dotnet-weak-crypto-sha1
    // SHA-512 for high-security integrity
    public byte[] HashIntegritySha512(string data)
    {
        using var sha512 = SHA512.Create();
        return sha512.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    // ok: dotnet-weak-crypto-des
    // AES-256 is the correct symmetric cipher
    public ICryptoTransform GetAesEncryptor(byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Key = key;
        aes.IV = iv;
        return aes.CreateEncryptor();
    }

    // ok: password hashing with a proper KDF
    public byte[] DeriveKeyPbkdf2(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password, salt, iterations: 600000,
            HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(32);
    }
}

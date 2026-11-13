using System.Security.Cryptography;
using System.Text;

public class WeakCryptoVulnerable
{
    // ruleid: dotnet-weak-crypto-md5
    public byte[] HashPasswordMd5(string password)
    {
        using var md5 = MD5.Create();
        return md5.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    // ruleid: dotnet-weak-crypto-sha1
    public byte[] HashTokenSha1(string token)
    {
        using var sha1 = SHA1.Create();
        return sha1.ComputeHash(Encoding.UTF8.GetBytes(token));
    }

    // ruleid: dotnet-weak-crypto-sha1
    public byte[] LegacySha1Managed(string data)
    {
        using var sha1 = new SHA1Managed();
        return sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
    }

    // ruleid: dotnet-weak-crypto-des
    public ICryptoTransform GetDesEncryptor(byte[] key, byte[] iv)
    {
        using var des = DES.Create();
        des.Key = key;
        des.IV = iv;
        return des.CreateEncryptor();
    }

    // ruleid: dotnet-weak-crypto-des
    public ICryptoTransform GetTripleDesEncryptor(byte[] key, byte[] iv)
    {
        using var tdes = TripleDES.Create();
        tdes.Key = key;
        tdes.IV = iv;
        return tdes.CreateEncryptor();
    }
}

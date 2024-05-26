using System.Security.Cryptography;
using System.Text;

namespace Domains;

public static class StringEncryptor
{
    public static byte[] Encrypt(string plainText)
    {
        var rsa = RSA.Create();
        var publicKeyText = File.ReadAllText("./cert/public_key.pem");
        rsa.ImportFromPem(publicKeyText);
        var dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
        return rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
    }
}
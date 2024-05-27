using System.Security.Cryptography;
using System.Text;

namespace Domains;

public static class StringEncryptor
{
    public static byte[] Encrypt(string plainText)
    {
        RSA rsa = RSA.Create();
        string publicKeyText = File.ReadAllText("./cert/public_key.pem");
        rsa.ImportFromPem(publicKeyText);
        byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
        return rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
    }
}
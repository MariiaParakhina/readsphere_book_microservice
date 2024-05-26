using System.Security.Cryptography;
using System.Text;

namespace Core;

public static class StringDecryptor
{
    public static string Decrypt(byte[] encryptedData)
    {
        var rsa = RSA.Create();
        var privateKeyText = File.ReadAllText("cert/private_key.pem");
        rsa.ImportFromEncryptedPem(privateKeyText, "pass");
        var decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decryptedData);
    }
}
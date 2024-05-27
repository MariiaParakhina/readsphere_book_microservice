using System.Security.Cryptography;
using System.Text;

namespace Core;

public static class StringDecryptor
{

    public static string Decrypt(byte[] encryptedData)
    {
        RSA rsa = RSA.Create();
        string privateKeyText = File.ReadAllText("cert/private_key.pem");
        rsa.ImportFromEncryptedPem(privateKeyText, "pass");
        byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decryptedData);

    }
}
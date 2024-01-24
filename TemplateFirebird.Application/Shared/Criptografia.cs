using System.Security.Cryptography;
using System.Text;

namespace TemplateFirebird.Application.Shared;

public class Criptografia : ICriptografia
{
    public static readonly string _keyString = "BLESS_SISTEMAS__API_SIDI";
    public static readonly string _vectorString = "abcede0123456789";

    public string Encrypt(string valor)
    {
        byte[] initializationVector = Encoding.ASCII.GetBytes(_vectorString);
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_keyString);
        aes.IV = initializationVector;
        var symmetricEncryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream as Stream, symmetricEncryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream as Stream))
        {
            streamWriter.Write(valor);
        }
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public string Decrypt(string valor)
    {
        byte[] initializationVector = Encoding.ASCII.GetBytes(_vectorString);
        byte[] buffer = Convert.FromBase64String(valor);
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_keyString);
        aes.IV = initializationVector;
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(memoryStream as Stream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream as Stream);
        return streamReader.ReadToEnd();
    }
}
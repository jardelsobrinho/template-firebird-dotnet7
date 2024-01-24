using System.Security.Cryptography;
using System.Text;

namespace TemplateFirebird.Domain.Usuario;

public class PasswordValueObject
{
    public string Value { get; set; } = "";
    public PasswordValueObject() { }
    public PasswordValueObject(string passwordDecrypt)
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(passwordDecrypt);
        byte[] hashBytes = MD5.HashData(inputBytes);
        Value = Convert.ToHexString(hashBytes);
    }
}
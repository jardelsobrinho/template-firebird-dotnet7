namespace TemplateFirebird.Application.Shared;

public interface ICriptografia
{
    string Encrypt(string valor);
    string Decrypt(string valor);
}

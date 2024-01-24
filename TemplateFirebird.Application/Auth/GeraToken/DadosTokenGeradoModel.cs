namespace TemplateFirebird.Application.Auth.GeraToken;

public class DadosTokenGeradoModel
{
    public required string Token { get; set; }
    public required DateTime TokenDataValidade { get; set; }
}

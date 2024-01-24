namespace TemplateFirebird.Application.Auth.RealizaLogin;

public class DadosLoginModel
{
    public required string Token { get; set; }
    public required string TokenRefresh { get; set; }
    public required DateTime TokenDataValidade { get; set; }
}

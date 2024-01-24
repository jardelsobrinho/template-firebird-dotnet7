namespace TemplateFirebird.Application.Auth;

public class DadosTokenModel
{
    public string? Erro { get; set; }
    public required string UsuarioNome { get; set; }
    public required int UsuarioCodigo { get; set; }
    public required string RepresentanteCnpj { get; set; }
    public required string EmpresaCnpj { get; set; }

    public static DadosTokenModel CriaDadosTokenComErro(string erro)
    {
        return new DadosTokenModel()
        {
            Erro = erro,
            UsuarioNome = "",
            UsuarioCodigo = 0,
            EmpresaCnpj = "",
            RepresentanteCnpj = ""
        };
    }
}
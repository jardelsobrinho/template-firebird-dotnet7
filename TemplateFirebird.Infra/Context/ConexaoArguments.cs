namespace TemplateFirebird.Infra.Context;

public class ConexaoArguments
{
    public string Datasource { get; set; } = "";
    public string Porta { get; set; } = "";
    public string Password { get; set; } = "";
    public string Database { get; set; } = "";
    public string Usuario { get; set; } = "";

    public static string RetornaValorArg(string argumento, string nome, string valor)
    {
        if (argumento.Contains(nome, StringComparison.CurrentCulture))
        {
            var conteudo = argumento.Split("=");
            if (conteudo.Length == 2)
            {
                return conteudo[1];
            }
        }
        return valor;
    }

    public ConexaoArguments(string[] listaArgumentos)
    {
        if (listaArgumentos.Length > 0)
        {
            foreach (string argumento in listaArgumentos)
            {
                Datasource = RetornaValorArg(argumento, "DATASOURCE", Datasource);
                Porta = RetornaValorArg(argumento, "PORTA", Porta);
                Password = RetornaValorArg(argumento, "PASSWORD", Password);
                Usuario = RetornaValorArg(argumento, "USUARIO", Usuario);
                Database = RetornaValorArg(argumento, "DATABASE", Database);
            }
        }

    }

}

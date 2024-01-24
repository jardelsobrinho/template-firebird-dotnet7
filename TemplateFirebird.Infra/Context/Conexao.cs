namespace TemplateFirebird.Infra.Context;

public static class Conexao
{
    public static ConexaoArguments? ConexaoArguments { get; set; }
    public static string RetornaString()
    {
        var dataSource = Environment.GetEnvironmentVariable("BS_DATASOURCE") ?? "LOCALHOST";
        var port = Environment.GetEnvironmentVariable("BS_PORT") ?? "3050";
        var password = Environment.GetEnvironmentVariable("BS_PASSWORD") ?? "pmpsyfwr";
        var database = Environment.GetEnvironmentVariable("BS_DATABASE") ?? "SIDI";
        var user = Environment.GetEnvironmentVariable("BS_USER") ?? "SYSDBA";

        if (ConexaoArguments != null)
        {
            if (ConexaoArguments.Database != "")
                database = ConexaoArguments.Database;

            if (ConexaoArguments.Porta != "")
                port = ConexaoArguments.Porta;

            if (ConexaoArguments.Usuario != "")
                user = ConexaoArguments.Usuario;

            if (ConexaoArguments.Password != "")
                password = ConexaoArguments.Password;

            if (ConexaoArguments.Datasource != "")
                dataSource = ConexaoArguments.Datasource;
        }

        return $"User={user};Password={password};Database={database};DataSource={dataSource};Port={port};Dialect=3;Charset=WIN1252;";
    }
}
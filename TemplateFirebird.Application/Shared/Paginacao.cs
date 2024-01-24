using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Text.RegularExpressions;

namespace TemplateFirebird.Application.Shared;

public class Paginacao<T> : IPaginacao<T>
{
    public IList<T> Registros { get; set; }
    public int Pagina { get; set; }
    public int RegistrosPorPagina { get; set; }
    public int TotalRegistros { get; set; }
    public int TotalPaginas { get; set; }
    public bool ExistePaginaAnterior => Pagina > 1;
    public bool ExisteProximaPagina => Pagina < TotalPaginas;

    public Paginacao(IList<T> regitros, int pagina, int registrosPorPagina, int totalRegistros)
    {
        if (pagina < 1)
            throw new BadHttpRequestException("PG02 - A pagina não pode ser menor que 1");

        Registros = regitros;
        Pagina = pagina;
        RegistrosPorPagina = registrosPorPagina;
        TotalRegistros = totalRegistros;
        TotalPaginas = (int)Math.Ceiling(TotalRegistros / (double)RegistrosPorPagina);

        if (Registros.Count == 0)
        {
            TotalPaginas = 0;
            Pagina = 1;
            TotalRegistros = 0;
        }
    }

    public static async Task<IPaginacao<T>> CriarPaginacaoAsync(string sqlSelect, string sqlFrom, string? sqlOrderBy, Dictionary<string, object> filtros, int pagina, int registrosPorPagina, IDbConnection conexao)
    {
        var sqlCount = "SELECT COUNT(*) " + sqlFrom;
        var parametrosCount = new DynamicParameters(filtros);
        var totalRegistros = await conexao.QueryFirstAsync<int>(sqlCount, parametrosCount);

        var regex = new Regex(Regex.Escape("SELECT"));
        var sqlComFiltros = regex.Replace(sqlSelect.ToUpper(), "SELECT FIRST @FIRST SKIP @SKIP ", 1);

        var sql = sqlComFiltros + " " + sqlFrom;
        if (sqlOrderBy != null)
        {
            sql += " " + sqlOrderBy;
        }

        filtros.Add("@FIRST", registrosPorPagina);

        var skip = pagina == 0 ? 1 : (pagina - 1) * registrosPorPagina;
        filtros.Add("@SKIP", skip);
        var parameters = new DynamicParameters(filtros);

        var registros = (await conexao.QueryAsync<T>(sql, parameters)).ToList();
        return new Paginacao<T>(registros, pagina, registrosPorPagina, totalRegistros);
    }
}
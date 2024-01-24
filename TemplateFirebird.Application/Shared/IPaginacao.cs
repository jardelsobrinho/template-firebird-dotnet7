namespace TemplateFirebird.Application.Shared;

public interface IPaginacao<T>
{
    public IList<T> Registros { get; }
    public int Pagina { get; set; }
    public int RegistrosPorPagina { get; set; }
    public int TotalRegistros { get; set; }
    public int TotalPaginas { get; set; }
    public bool ExistePaginaAnterior { get; }
    public bool ExisteProximaPagina { get; }
}

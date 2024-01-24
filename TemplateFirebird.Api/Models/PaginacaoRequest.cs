namespace TemplateFirebird.Api.Models;

public class PaginacaoRequest
{
    public required int Pagina { get; set; } = 1;
    public required int RegistrosPorPagina { get; set; } = 12;
}

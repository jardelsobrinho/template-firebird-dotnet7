using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TemplateFirebird.Domain.Usuario;
using TemplateFirebird.Infra.Context;

namespace TemplateFirebird.Infra.Repositories;

public class UsuarioRepository : Repository<UsuarioEntity>, IUsuarioRepository
{
    public UsuarioRepository(SidiDbContext context) : base(context)
    {
    }

    public async Task<UsuarioEntity?> CarregaDadosAsync(Expression<Func<UsuarioEntity, bool>> expression)
    {
        var consulta = await _context.Usuarios
            .Where(expression)
            .SingleOrDefaultAsync();
        return consulta;
    }
}
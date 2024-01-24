using System.Linq.Expressions;
using TemplateFirebird.Domain.Shared;

namespace TemplateFirebird.Domain.Usuario
{
    public interface IUsuarioRepository : IRepository<UsuarioEntity>
    {
        Task<UsuarioEntity?> CarregaDadosAsync(Expression<Func<UsuarioEntity, bool>> expression);
    }
}
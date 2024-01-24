using System.Linq.Expressions;

namespace TemplateFirebird.Domain.Shared;

public interface IRepository<T>
{
    Task<T> GravaAsync(T entity, int codigo);
    Task ExcluirAsync(Expression<Func<T, bool>> expression);
    Task<IList<T>> PesquisaAsync(Expression<Func<T, bool>> expression, bool hasNoTracking = true);
    Task<IList<T>> PesquisaAsync(List<Expression<Func<T, bool>>> expressions, bool hasNoTracking = true);
}

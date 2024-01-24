using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using TemplateFirebird.Domain.Shared;
using TemplateFirebird.Infra.Context;
using TemplateFirebird.Infra.Exceptions;

namespace TemplateFirebird.Infra.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected SidiDbContext _context;

    public IDbContextTransaction StartTransaction()
    {
        return _context.Database.BeginTransaction();
    }

    public Repository(SidiDbContext context)
    {
        _context = context;
    }

    public async Task ExcluirAsync(Expression<Func<T, bool>> expression)
    {
        var entity = _context.Set<T>().Where(expression).SingleOrDefault() ?? throw new EntidadeNaoEncontradaException("RP01 - Registro não encontrado");
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T> GravaAsync(T entity, int codigo)
    {
        if (codigo == 0)
        {
            await _context.AddAsync(entity);
        }
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IList<T>> PesquisaAsync(List<Expression<Func<T, bool>>> expressions, bool hasNoTracking = true)
    {
        IQueryable<T> consulta = _context.Set<T>();

        if (hasNoTracking)
        {
            consulta = consulta.AsNoTracking();
        }

        foreach (var e in expressions)
        {
            consulta = consulta.Where(e);
        }

        return await consulta.ToListAsync();
    }

    public Task AdicionaAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public void AtualizaAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<T>> PesquisaAsync(Expression<Func<T, bool>> expression, bool hasNoTracking = true)
    {
        return await PesquisaAsync(new List<Expression<Func<T, bool>>> { expression }, hasNoTracking);
    }
}

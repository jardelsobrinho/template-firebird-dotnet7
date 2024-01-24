using Microsoft.EntityFrameworkCore.Storage;
using TemplateFirebird.Domain.Shared;
using TemplateFirebird.Domain.Usuario;
using TemplateFirebird.Infra.Context;

namespace TemplateFirebird.Infra.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public IUsuarioRepository UsuarioRepository { get; }
    private readonly SidiDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(SidiDbContext context, IUsuarioRepository usuarioRepository)
    {
        _context = context;
        UsuarioRepository = usuarioRepository;
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public async Task StartTransactionAsync()
    {
        if (_transaction != null)
            throw new Exception("UOW01 - Já existe uma transação iniciada");

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _transaction!.CommitAsync();
        _transaction.Dispose();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction!.RollbackAsync();
        _transaction.Dispose();
        _transaction = null;
    }
}

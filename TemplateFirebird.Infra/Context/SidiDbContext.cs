using Microsoft.EntityFrameworkCore;
using TemplateFirebird.Domain.Usuario;

namespace TemplateFirebird.Infra.Context;

public class SidiDbContext : DbContext
{
    public DbSet<UsuarioEntity> Usuarios => Set<UsuarioEntity>();
    public SidiDbContext() { }
    public SidiDbContext(DbContextOptions<SidiDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SidiDbContext).Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseFirebird(Conexao.RetornaString());
        }
    }
}

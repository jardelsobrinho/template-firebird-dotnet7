using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using TemplateFirebird.Application.Shared;
using TemplateFirebird.Domain.Shared;
using TemplateFirebird.Domain.Usuario;
using TemplateFirebird.Infra.Context;
using TemplateFirebird.Infra.Repositories;

namespace TemplateFirebird.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, string[] args)
        {
            Conexao.ConexaoArguments = new ConexaoArguments(args);

            services.AddDbContext<SidiDbContext>();
            services.AddScoped<IDbConnection>((sp) => new FbConnection(Conexao.RetornaString()));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICriptografia, Criptografia>();
            return services;
        }
    }

}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateFirebird.Domain.Usuario;

namespace TemplateFirebird.Infra.Maps;

public class UsuarioMap : IEntityTypeConfiguration<UsuarioEntity>
{
    public void Configure(EntityTypeBuilder<UsuarioEntity> builder)
    {
        builder.ToTable("USUARIO");
        builder.HasKey(x => x.Codigo);
        builder.Property(x => x.Codigo)
            .HasColumnName("COD_USUARIO")
            .HasColumnType("INTEGER")
            .IsRequired(); ;

        builder.Property(x => x.CnpjRepresentante)
            .HasColumnName("CGC_REPRESENTANTE")
            .HasColumnType("VARCHAR(14)");

        builder.Property(x => x.Usuario)
            .HasColumnName("NOME")
            .HasColumnType("VARCHAR(15)")
            .IsRequired();

        builder.Property(x => x.NomeCompleto)
            .HasColumnName("NOME_COMPLETO")
            .HasColumnType("VARCHAR(40)")
            .IsRequired();

        builder.OwnsOne(x => x.Password)
            .Property(x => x.Value)
            .HasColumnName("SENHA")
            .HasColumnType("VARCHAR(255)")
            .IsRequired(true);

    }
}
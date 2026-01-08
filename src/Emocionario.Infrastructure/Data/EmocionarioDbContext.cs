using Emocionario.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace Emocionario.Infrastructure.Data;

public class EmocionarioDbContext : DbContext
{
    public EmocionarioDbContext(DbContextOptions<EmocionarioDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Nome)
                .HasConversion(
                    nome => nome.Valor,
                    valor => new Domain.Usuarios.ValueObjects.PrimeiroNome(valor))
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.Sobrenome)
                .HasConversion(
                    sobrenome => sobrenome.Valor,
                    valor => new Domain.Usuarios.ValueObjects.Sobrenome(valor))
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.Email)
                .HasConversion(
                    email => email.Valor,
                    valor => new Domain.Usuarios.ValueObjects.Email(valor))
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.DataNascimento)
                .HasConversion(
                    data => data.HasValue ? data.Value.Valor : (DateOnly?)null,
                    valor => valor.HasValue ? new Domain.Usuarios.ValueObjects.DataNascimento(valor.Value) : null);

            entity.Property(u => u.CriadoEm)
                .IsRequired();

            entity.Property(u => u.AtualizadoEm);
        });
    }
}

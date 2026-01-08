using Emocionario.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace Emocionario.Infrastructure.Data;

/// <summary>
/// Contexto do Entity Framework Core para a aplicação Emocionario.
/// Gerencia a conexão com o banco de dados e o mapeamento de entidades de domínio.
/// </summary>
/// <remarks>
/// Este DbContext é responsável por:
/// - Gerenciar a conexão com o banco de dados (atualmente InMemory)
/// - Mapear entidades de domínio para tabelas do banco de dados
/// - Configurar conversões de Value Objects para tipos primitivos do banco
/// - Definir constraints, índices e relacionamentos
///
/// Padrões implementados:
/// - Unit of Work: DbContext gerencia transações e mudanças
/// - Repository Pattern: Fornece DbSets que são consumidos pelos Repositories
/// - Value Object Conversion: Converte automaticamente entre Value Objects e tipos primitivos
///
/// Configuração atual:
/// - Database: InMemory (para desenvolvimento e testes)
/// - Entidades mapeadas: Usuario
/// </remarks>
public class EmocionarioDbContext : DbContext
{
    /// <summary>
    /// Inicializa uma nova instância do contexto do banco de dados.
    /// </summary>
    /// <param name="options">Opções de configuração do DbContext (provedor, connection string, etc.).</param>
    public EmocionarioDbContext(DbContextOptions<EmocionarioDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Obtém o DbSet de usuários para operações de consulta e persistência.
    /// </summary>
    /// <remarks>
    /// Este DbSet é usado pelos repositories para executar queries LINQ
    /// e operações CRUD sobre a entidade Usuario.
    /// </remarks>
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    /// <summary>
    /// Configura o modelo de dados usando Fluent API.
    /// Define mapeamentos, conversões de Value Objects, constraints e índices.
    /// </summary>
    /// <param name="modelBuilder">Builder usado para construir o modelo de dados.</param>
    /// <remarks>
    /// Configurações aplicadas:
    ///
    /// Para a entidade Usuario:
    /// - Chave primária: Id (Guid)
    /// - Value Objects convertidos para tipos primitivos:
    ///   * PrimeiroNome → string (max 100 chars, required)
    ///   * Sobrenome → string (max 100 chars, required)
    ///   * Email → string (max 255 chars, required, unique index)
    ///   * DataNascimento → DateOnly? (nullable)
    /// - Campos de auditoria:
    ///   * CriadoEm → DateTime (required)
    ///   * AtualizadoEm → DateTime? (optional)
    /// - Índices:
    ///   * Email (unique) - garante unicidade do e-mail no banco
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            // Chave primária
            entity.HasKey(u => u.Id);

            // Value Object: PrimeiroNome → string
            // Conversão bidirecional entre o Value Object e o tipo primitivo do banco
            entity.Property(u => u.Nome)
                .HasConversion(
                    nome => nome.Valor,                                              // Domínio → Banco
                    valor => new Domain.Usuarios.ValueObjects.PrimeiroNome(valor))   // Banco → Domínio
                .HasMaxLength(100)
                .IsRequired();

            // Value Object: Sobrenome → string
            // Conversão bidirecional entre o Value Object e o tipo primitivo do banco
            entity.Property(u => u.Sobrenome)
                .HasConversion(
                    sobrenome => sobrenome.Valor,                                    // Domínio → Banco
                    valor => new Domain.Usuarios.ValueObjects.Sobrenome(valor))      // Banco → Domínio
                .HasMaxLength(100)
                .IsRequired();

            // Value Object: Email → string
            // Conversão bidirecional entre o Value Object e o tipo primitivo do banco
            entity.Property(u => u.Email)
                .HasConversion(
                    email => email.Valor,                                            // Domínio → Banco
                    valor => new Domain.Usuarios.ValueObjects.Email(valor))          // Banco → Domínio
                .HasMaxLength(255)
                .IsRequired();

            // Índice único para garantir que não existam dois usuários com o mesmo e-mail
            entity.HasIndex(u => u.Email).IsUnique();

            // Value Object: DataNascimento → DateOnly? (nullable)
            // Conversão bidirecional tratando valores nulos
            entity.Property(u => u.DataNascimento)
                .HasConversion(
                    data => data.HasValue ? data.Value.Valor : (DateOnly?)null,                              // Domínio → Banco
                    valor => valor.HasValue ? new Domain.Usuarios.ValueObjects.DataNascimento(valor.Value) : null);  // Banco → Domínio

            // Campo de auditoria: data de criação (obrigatório)
            entity.Property(u => u.CriadoEm)
                .IsRequired();

            // Campo de auditoria: data de atualização (opcional - null quando nunca atualizado)
            entity.Property(u => u.AtualizadoEm);
        });
    }
}

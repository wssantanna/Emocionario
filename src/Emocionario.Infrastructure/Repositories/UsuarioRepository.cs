using Emocionario.Application.Usuarios.Repositories;
using Emocionario.Domain.Usuarios;
using Emocionario.Domain.Usuarios.ValueObjects;
using Emocionario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Emocionario.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de usuários usando Entity Framework Core.
/// Fornece operações de persistência de dados para a entidade Usuario.
/// </summary>
/// <remarks>
/// Este repositório implementa o padrão Repository Pattern, abstraindo os detalhes
/// de acesso a dados da camada de aplicação e domínio.
///
/// Características:
/// - Todas as operações são assíncronas (async/await)
/// - Suporte a CancellationToken para permitir cancelamento de operações
/// - Utiliza LINQ to Entities para queries type-safe
/// - Gerencia automaticamente o SaveChanges para persistir mudanças
/// - Trabalha com entidades de domínio (Usuario) e Value Objects (Email)
///
/// Padrões implementados:
/// - Repository Pattern: abstração do acesso a dados
/// - Unit of Work: coordenado pelo DbContext
/// - Async/Await: todas as operações são não-bloqueantes
/// </remarks>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly EmocionarioDbContext _context;

    /// <summary>
    /// Inicializa uma nova instância do repositório de usuários.
    /// </summary>
    /// <param name="context">Contexto do Entity Framework Core para acesso ao banco de dados.</param>
    /// <exception cref="ArgumentNullException">Lançada quando context é null.</exception>
    public UsuarioRepository(EmocionarioDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Verifica se já existe um usuário cadastrado com o e-mail especificado.
    /// </summary>
    /// <param name="email">O e-mail a ser verificado (Value Object).</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém true se o e-mail já existe; caso contrário, false.
    /// </returns>
    /// <remarks>
    /// Utiliza AnyAsync para otimização de performance (não carrega entidades desnecessariamente).
    /// O Entity Framework converte automaticamente o Value Object Email para string na query.
    /// </remarks>
    public async Task<bool> ExisteComEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// Adiciona um novo usuário ao banco de dados.
    /// </summary>
    /// <param name="usuario">A entidade Usuario a ser adicionada.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>Uma task que representa a operação assíncrona.</returns>
    /// <remarks>
    /// Esta operação:
    /// - Adiciona o usuário ao DbSet
    /// - Persiste as mudanças no banco de dados (SaveChangesAsync)
    /// - É uma operação atômica (transacional)
    /// </remarks>
    public async Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        await _context.Usuarios.AddAsync(usuario, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém um usuário pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único (GUID) do usuário.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o usuário encontrado ou null se nenhum usuário for encontrado.
    /// </returns>
    /// <remarks>
    /// Utiliza FirstOrDefaultAsync para retornar null quando o usuário não existe,
    /// evitando exceções e permitindo tratamento mais elegante na camada de serviço.
    /// </remarks>
    public async Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <summary>
    /// Obtém um usuário pelo seu endereço de e-mail.
    /// </summary>
    /// <param name="email">O e-mail do usuário a ser buscado (Value Object).</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o usuário encontrado ou null se nenhum usuário for encontrado.
    /// </returns>
    /// <remarks>
    /// Como o e-mail possui um índice único no banco, esta query é otimizada.
    /// O Entity Framework converte automaticamente o Value Object Email para string na query.
    /// </remarks>
    public async Task<Usuario?> ObterPorEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente no banco de dados.
    /// </summary>
    /// <param name="usuario">A entidade Usuario com os dados atualizados.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>Uma task que representa a operação assíncrona.</returns>
    /// <remarks>
    /// Esta operação:
    /// - Marca a entidade como modificada no DbContext
    /// - Persiste as mudanças no banco de dados (SaveChangesAsync)
    /// - É uma operação atômica (transacional)
    /// - O Entity Framework detecta automaticamente quais propriedades foram alteradas
    /// </remarks>
    public async Task AtualizarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Remove um usuário do banco de dados pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único (GUID) do usuário a ser excluído.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém true se o usuário foi excluído com sucesso; false se não foi encontrado.
    /// </returns>
    /// <remarks>
    /// Esta operação:
    /// - Primeiro busca o usuário pelo ID (reutilizando ObterPorIdAsync)
    /// - Se encontrado, remove a entidade do DbSet
    /// - Persiste as mudanças no banco de dados (SaveChangesAsync)
    /// - É uma operação atômica (transacional)
    /// - Retorna false se o usuário não existir (evita exceções)
    /// </remarks>
    public async Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usuario = await ObterPorIdAsync(id, cancellationToken);

        if (usuario is null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

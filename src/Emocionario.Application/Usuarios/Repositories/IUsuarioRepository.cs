using Emocionario.Domain.Usuarios;
using Emocionario.Domain.Usuarios.ValueObjects;

namespace Emocionario.Application.Usuarios.Repositories;

/// <summary>
/// Define o contrato para operações de persistência de usuários.
/// Implementa o padrão Repository para abstrair o acesso a dados da camada de domínio.
/// </summary>
/// <remarks>
/// Este repositório é responsável por:
/// - Isolar a lógica de acesso a dados da lógica de negócios
/// - Fornecer uma interface orientada a domínio (usando entidades e value objects)
/// - Permitir diferentes implementações de persistência (EF Core, Dapper, etc.)
/// - Suportar operações assíncronas com CancellationToken
/// </remarks>
public interface IUsuarioRepository
{
    /// <summary>
    /// Verifica se já existe um usuário cadastrado com o e-mail especificado.
    /// </summary>
    /// <param name="email">O e-mail a ser verificado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém true se o e-mail já existe; caso contrário, false.
    /// </returns>
    /// <remarks>
    /// Este método é útil para validar a unicidade do e-mail antes de criar ou atualizar um usuário.
    /// </remarks>
    Task<bool> ExisteComEmailAsync(Email email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona um novo usuário ao repositório.
    /// </summary>
    /// <param name="usuario">A entidade Usuario a ser adicionada.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>Uma task que representa a operação assíncrona.</returns>
    /// <remarks>
    /// O usuário deve ser criado através do método Usuario.Criar() para garantir que todas as validações sejam executadas.
    /// </remarks>
    Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um usuário pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único (GUID) do usuário.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o usuário encontrado ou null se nenhum usuário for encontrado com o ID especificado.
    /// </returns>
    Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um usuário pelo seu endereço de e-mail.
    /// </summary>
    /// <param name="email">O e-mail do usuário a ser buscado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o usuário encontrado ou null se nenhum usuário for encontrado com o e-mail especificado.
    /// </returns>
    /// <remarks>
    /// Como o e-mail é único no sistema, este método retorna no máximo um usuário.
    /// </remarks>
    Task<Usuario?> ObterPorEmailAsync(Email email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza os dados de um usuário existente no repositório.
    /// </summary>
    /// <param name="usuario">A entidade Usuario com os dados atualizados.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>Uma task que representa a operação assíncrona.</returns>
    /// <remarks>
    /// O usuário deve ser atualizado através do método Usuario.Atualizar() para garantir que todas as validações sejam executadas.
    /// A propriedade AtualizadoEm será automaticamente atualizada pela entidade.
    /// </remarks>
    Task AtualizarAsync(Usuario usuario, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um usuário do repositório pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único (GUID) do usuário a ser excluído.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém true se o usuário foi excluído com sucesso; caso contrário, false (usuário não encontrado).
    /// </returns>
    Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default);
}

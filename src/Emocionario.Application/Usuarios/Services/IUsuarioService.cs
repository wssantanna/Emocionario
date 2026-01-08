using Emocionario.Application.Usuarios.DTOs;

namespace Emocionario.Application.Usuarios.Services;

/// <summary>
/// Define o contrato para operações de negócio relacionadas a usuários.
/// Implementa o padrão Service Layer, orquestrando operações entre DTOs, domínio e repositório.
/// </summary>
/// <remarks>
/// Este serviço é responsável por:
/// - Validar regras de negócio (como unicidade de e-mail)
/// - Converter DTOs em entidades de domínio e vice-versa
/// - Orquestrar operações entre a camada de apresentação e o domínio
/// - Garantir que as operações de domínio sejam executadas de forma consistente
/// - Fornecer uma API orientada a DTOs para a camada de apresentação/API
/// </remarks>
public interface IUsuarioService
{
    /// <summary>
    /// Cria um novo usuário no sistema.
    /// </summary>
    /// <param name="dto">DTO contendo os dados do usuário a ser criado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o DTO do usuário criado, incluindo o ID gerado.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando já existe um usuário cadastrado com o e-mail fornecido.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados do DTO violam as regras de validação dos Value Objects
    /// (ex: nome muito curto, e-mail inválido, data de nascimento futura).
    /// </exception>
    /// <remarks>
    /// Este método:
    /// - Valida a unicidade do e-mail antes de criar o usuário
    /// - Converte strings do DTO em Value Objects do domínio
    /// - Utiliza o Factory Method Usuario.Criar() para garantir criação válida
    /// - Retorna o usuário criado com todas as propriedades, incluindo CriadoEm
    /// </remarks>
    Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um usuário pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único (GUID) do usuário.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o DTO do usuário encontrado ou null se nenhum usuário for encontrado.
    /// </returns>
    Task<UsuarioDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um usuário pelo seu endereço de e-mail.
    /// </summary>
    /// <param name="email">O endereço de e-mail do usuário (string).</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o DTO do usuário encontrado ou null se nenhum usuário for encontrado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o formato do e-mail é inválido (não contém @ ou .).
    /// </exception>
    /// <remarks>
    /// O e-mail fornecido como string será convertido para o Value Object Email,
    /// aplicando normalização (lowercase) antes da busca.
    /// </remarks>
    Task<UsuarioDto?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// </summary>
    /// <param name="dto">DTO contendo o ID do usuário e os campos a serem atualizados.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém true se o usuário foi atualizado com sucesso; false se o usuário não foi encontrado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando os novos dados violam as regras de validação dos Value Objects
    /// (ex: nome muito curto, data de nascimento futura).
    /// </exception>
    /// <remarks>
    /// Este método suporta atualizações parciais:
    /// - Campos nulos ou vazios no DTO manterão os valores atuais
    /// - Apenas campos fornecidos serão atualizados
    /// - O e-mail NÃO pode ser atualizado (não está presente no AtualizarUsuarioDto)
    /// - A propriedade AtualizadoEm é atualizada automaticamente
    /// </remarks>
    Task<bool> AtualizarAsync(AtualizarUsuarioDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um usuário do sistema pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único (GUID) do usuário a ser excluído.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém true se o usuário foi excluído com sucesso; false se o usuário não foi encontrado.
    /// </returns>
    Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default);
}

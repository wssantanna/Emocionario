namespace Emocionario.Application.Usuarios.DTOs;

/// <summary>
/// Data Transfer Object (DTO) que representa os dados completos de um usuário.
/// Usado como resposta em operações de consulta (GET) na camada de apresentação/API.
/// </summary>
/// <remarks>
/// Este DTO é usado para:
/// - Transferir dados do usuário da camada de aplicação para a camada de apresentação
/// - Serialização em respostas HTTP/JSON
/// - Proteger a camada de domínio de ser exposta diretamente
/// - Fornecer uma estrutura de dados plana e simples (usando tipos primitivos)
///
/// Implementado como sealed record para garantir:
/// - Imutabilidade: propriedades com init-only setters
/// - Igualdade por valor: comparação baseada nos valores das propriedades
/// - Sintaxe concisa com inicializadores de objeto
/// </remarks>
public sealed record UsuarioDto
{
    /// <summary>
    /// Identificador único do usuário.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Primeiro nome do usuário.
    /// </summary>
    /// <remarks>
    /// Este valor já foi validado e normalizado pela camada de domínio (Value Object PrimeiroNome).
    /// </remarks>
    public required string Nome { get; init; }

    /// <summary>
    /// Sobrenome do usuário.
    /// </summary>
    /// <remarks>
    /// Este valor já foi validado e normalizado pela camada de domínio (Value Object Sobrenome).
    /// </remarks>
    public required string Sobrenome { get; init; }

    /// <summary>
    /// Endereço de e-mail do usuário (normalizado em minúsculas).
    /// </summary>
    /// <remarks>
    /// Este valor já foi validado e normalizado pela camada de domínio (Value Object Email).
    /// O e-mail é único no sistema.
    /// </remarks>
    public required string Email { get; init; }

    /// <summary>
    /// Data de nascimento do usuário (opcional).
    /// </summary>
    /// <remarks>
    /// Pode ser null quando o usuário não forneceu a data de nascimento.
    /// </remarks>
    public DateOnly? DataNascimento { get; init; }

    /// <summary>
    /// Data e hora (UTC) em que o usuário foi criado no sistema.
    /// </summary>
    public required DateTime CriadoEm { get; init; }

    /// <summary>
    /// Data e hora (UTC) da última atualização dos dados do usuário.
    /// </summary>
    /// <remarks>
    /// Será null quando o usuário nunca foi atualizado desde a criação.
    /// </remarks>
    public DateTime? AtualizadoEm { get; init; }
}

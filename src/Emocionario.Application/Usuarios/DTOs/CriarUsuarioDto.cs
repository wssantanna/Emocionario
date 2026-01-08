namespace Emocionario.Application.Usuarios.DTOs;

/// <summary>
/// Data Transfer Object (DTO) para criação de novos usuários.
/// Usado como entrada em operações de criação (POST) na camada de apresentação/API.
/// </summary>
/// <remarks>
/// Este DTO é usado para:
/// - Receber dados do cliente para criar um novo usuário
/// - Validar os dados de entrada antes de criar a entidade de domínio
/// - Desacoplar a camada de apresentação da camada de domínio
///
/// Implementado como sealed record para garantir:
/// - Imutabilidade: propriedades com init-only setters
/// - Igualdade por valor: comparação baseada nos valores das propriedades
/// - Sintaxe concisa com inicializadores de objeto
///
/// Regras de validação (aplicadas pela camada de domínio):
/// - Nome: obrigatório, 3-50 caracteres
/// - Sobrenome: obrigatório, 3-50 caracteres
/// - Email: obrigatório, formato válido (contém @ e .), único no sistema
/// - DataNascimento: opcional, não pode ser data futura
/// </remarks>
public sealed record CriarUsuarioDto
{
    /// <summary>
    /// Primeiro nome do usuário.
    /// </summary>
    /// <remarks>
    /// Obrigatório. Será validado e normalizado pelo Value Object PrimeiroNome na camada de domínio.
    /// Deve ter entre 3 e 50 caracteres após normalização (Trim).
    /// </remarks>
    public required string Nome { get; init; }

    /// <summary>
    /// Sobrenome do usuário.
    /// </summary>
    /// <remarks>
    /// Obrigatório. Será validado e normalizado pelo Value Object Sobrenome na camada de domínio.
    /// Deve ter entre 3 e 50 caracteres após normalização (Trim).
    /// </remarks>
    public required string Sobrenome { get; init; }

    /// <summary>
    /// Endereço de e-mail do usuário.
    /// </summary>
    /// <remarks>
    /// Obrigatório. Será validado e normalizado pelo Value Object Email na camada de domínio.
    /// Deve conter @ e . (validação básica de formato).
    /// Será convertido para minúsculas automaticamente.
    /// Deve ser único no sistema (verificado pela camada de aplicação).
    /// </remarks>
    public required string Email { get; init; }

    /// <summary>
    /// Data de nascimento do usuário (opcional).
    /// </summary>
    /// <remarks>
    /// Opcional. Se fornecida, será validada pelo Value Object DataNascimento na camada de domínio.
    /// Não pode ser uma data futura.
    /// </remarks>
    public DateOnly? DataNascimento { get; init; }
}

namespace Emocionario.Application.Usuarios.DTOs;

/// <summary>
/// Data Transfer Object (DTO) para atualização de usuários existentes.
/// Usado como entrada em operações de atualização (PUT/PATCH) na camada de apresentação/API.
/// </summary>
/// <remarks>
/// Este DTO é usado para:
/// - Receber dados do cliente para atualizar um usuário existente
/// - Suportar atualizações parciais (campos opcionais)
/// - Desacoplar a camada de apresentação da camada de domínio
///
/// Implementado como sealed record para garantir:
/// - Imutabilidade: propriedades com init-only setters
/// - Igualdade por valor: comparação baseada nos valores das propriedades
/// - Sintaxe concisa com inicializadores de objeto
///
/// Características importantes:
/// - Suporta atualização parcial: campos opcionais (nullable) mantêm valores atuais se não fornecidos
/// - O e-mail NÃO pode ser atualizado (não está presente neste DTO - regra de negócio)
/// - Apenas ID é obrigatório para identificar o usuário a ser atualizado
///
/// Regras de validação (aplicadas pela camada de domínio quando fornecidos):
/// - Nome: se fornecido, deve ter 3-50 caracteres
/// - Sobrenome: se fornecido, deve ter 3-50 caracteres
/// - DataNascimento: se fornecida, não pode ser data futura
/// </remarks>
public sealed record AtualizarUsuarioDto
{
    /// <summary>
    /// Identificador único do usuário a ser atualizado.
    /// </summary>
    /// <remarks>
    /// Obrigatório. Usado para localizar o usuário existente no sistema.
    /// </remarks>
    public required Guid Id { get; init; }

    /// <summary>
    /// Novo primeiro nome do usuário (opcional).
    /// </summary>
    /// <remarks>
    /// Opcional. Se fornecido (não null ou vazio), será validado e normalizado pelo Value Object PrimeiroNome.
    /// Deve ter entre 3 e 50 caracteres após normalização (Trim).
    /// Se não fornecido ou vazio, o nome atual do usuário será mantido.
    /// </remarks>
    public string? Nome { get; init; }

    /// <summary>
    /// Novo sobrenome do usuário (opcional).
    /// </summary>
    /// <remarks>
    /// Opcional. Se fornecido (não null ou vazio), será validado e normalizado pelo Value Object Sobrenome.
    /// Deve ter entre 3 e 50 caracteres após normalização (Trim).
    /// Se não fornecido ou vazio, o sobrenome atual do usuário será mantido.
    /// </remarks>
    public string? Sobrenome { get; init; }

    /// <summary>
    /// Nova data de nascimento do usuário (opcional).
    /// </summary>
    /// <remarks>
    /// Opcional. Se fornecida, será validada pelo Value Object DataNascimento.
    /// Não pode ser uma data futura.
    /// Se não fornecida, a data de nascimento atual do usuário será mantida.
    /// </remarks>
    public DateOnly? DataNascimento { get; init; }
}

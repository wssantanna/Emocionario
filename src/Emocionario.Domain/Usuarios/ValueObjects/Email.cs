namespace Emocionario.Domain.Usuarios.ValueObjects;

/// <summary>
/// Value Object que representa um endereço de e-mail de usuário.
/// Garante a imutabilidade e as regras de validação para endereços de e-mail.
/// </summary>
/// <remarks>
/// Implementado como readonly record struct para garantir:
/// - Imutabilidade: uma vez criado, o valor não pode ser alterado
/// - Igualdade por valor: dois Email são iguais se possuem o mesmo valor
/// - Performance: struct evita alocação no heap
///
/// Regras de validação:
/// - Não pode ser vazio ou conter apenas espaços
/// - Deve conter o caractere "@" (validação básica de formato)
/// - Deve conter o caractere "." (validação básica de domínio)
///
/// Normalização:
/// - Remove espaços em branco no início e fim (Trim)
/// - Converte para minúsculas (ToLowerInvariant) para garantir consistência
/// </remarks>
public readonly record struct Email
{
    /// <summary>
    /// Obtém o valor do e-mail normalizado (em minúsculas).
    /// </summary>
    public string Valor { get; }

    /// <summary>
    /// Inicializa uma nova instância de Email com validações e normalização.
    /// </summary>
    /// <param name="valor">O endereço de e-mail a ser encapsulado.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando:
    /// - O valor é nulo, vazio ou contém apenas espaços em branco
    /// - O valor não contém o caractere "@"
    /// - O valor não contém o caractere "." (ponto)
    /// </exception>
    public Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("O email não pode estar vazio.", nameof(valor));

        // Normalização: remove espaços e converte para minúsculas
        var emailNormalizado = valor.Trim().ToLowerInvariant();

        // Validação básica de formato de e-mail
        if (!emailNormalizado.Contains("@") || !emailNormalizado.Contains("."))
            throw new ArgumentException("O email fornecido não é válido.", nameof(valor));

        Valor = emailNormalizado;
    }

    /// <summary>
    /// Retorna a representação em string do e-mail.
    /// </summary>
    /// <returns>O valor do e-mail normalizado.</returns>
    public override string ToString() => Valor;

    /// <summary>
    /// Conversão implícita de Email para string.
    /// Permite usar Email diretamente onde string é esperado.
    /// </summary>
    /// <param name="email">O Email a ser convertido.</param>
    public static implicit operator string(Email email) => email.Valor;
}

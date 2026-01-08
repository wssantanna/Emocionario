namespace Emocionario.Domain.Usuarios.ValueObjects;

/// <summary>
/// Value Object que representa o primeiro nome de um usuário.
/// Garante a imutabilidade e as regras de validação para o primeiro nome.
/// </summary>
/// <remarks>
/// Implementado como readonly record struct para garantir:
/// - Imutabilidade: uma vez criado, o valor não pode ser alterado
/// - Igualdade por valor: dois PrimeiroNome são iguais se possuem o mesmo valor
/// - Performance: struct evita alocação no heap
///
/// Regras de validação:
/// - Não pode ser vazio ou conter apenas espaços
/// - Deve ter no mínimo 3 caracteres (após normalização)
/// - Deve ter no máximo 50 caracteres (após normalização)
/// - O valor é normalizado (Trim) antes de ser armazenado
/// </remarks>
public readonly record struct PrimeiroNome
{
    /// <summary>
    /// Obtém o valor do primeiro nome normalizado.
    /// </summary>
    public string Valor { get; }

    /// <summary>
    /// Inicializa uma nova instância de PrimeiroNome com validações.
    /// </summary>
    /// <param name="valor">O primeiro nome a ser encapsulado.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando:
    /// - O valor é nulo, vazio ou contém apenas espaços em branco
    /// - O valor normalizado possui menos de 3 caracteres
    /// - O valor normalizado possui mais de 50 caracteres
    /// </exception>
    public PrimeiroNome(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("O primeiro nome não pode estar vazio.", nameof(valor));

        var nomeNormalizado = valor.Trim();

        if (nomeNormalizado.Length < 3)
            throw new ArgumentException("O primeiro nome deve ter no mínimo 3 caracteres.", nameof(valor));

        if (nomeNormalizado.Length > 50)
            throw new ArgumentException("O primeiro nome deve ter no máximo 50 caracteres.", nameof(valor));

        Valor = nomeNormalizado;
    }

    /// <summary>
    /// Retorna a representação em string do primeiro nome.
    /// </summary>
    /// <returns>O valor do primeiro nome.</returns>
    public override string ToString() => Valor;

    /// <summary>
    /// Conversão implícita de PrimeiroNome para string.
    /// Permite usar PrimeiroNome diretamente onde string é esperado.
    /// </summary>
    /// <param name="primeiroNome">O PrimeiroNome a ser convertido.</param>
    public static implicit operator string(PrimeiroNome primeiroNome) => primeiroNome.Valor;
}

namespace Emocionario.Domain.Usuarios.ValueObjects;

/// <summary>
/// Value Object que representa o sobrenome de um usuário.
/// Garante a imutabilidade e as regras de validação para o sobrenome.
/// </summary>
/// <remarks>
/// Implementado como readonly record struct para garantir:
/// - Imutabilidade: uma vez criado, o valor não pode ser alterado
/// - Igualdade por valor: dois Sobrenome são iguais se possuem o mesmo valor
/// - Performance: struct evita alocação no heap
///
/// Regras de validação:
/// - Não pode ser vazio ou conter apenas espaços
/// - Deve ter no mínimo 3 caracteres (após normalização)
/// - Deve ter no máximo 50 caracteres (após normalização)
/// - O valor é normalizado (Trim) antes de ser armazenado
/// </remarks>
public readonly record struct Sobrenome
{
    /// <summary>
    /// Obtém o valor do sobrenome normalizado.
    /// </summary>
    public string Valor { get; }

    /// <summary>
    /// Inicializa uma nova instância de Sobrenome com validações.
    /// </summary>
    /// <param name="valor">O sobrenome a ser encapsulado.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando:
    /// - O valor é nulo, vazio ou contém apenas espaços em branco
    /// - O valor normalizado possui menos de 3 caracteres
    /// - O valor normalizado possui mais de 50 caracteres
    /// </exception>
    public Sobrenome(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("O sobrenome não pode estar vazio.", nameof(valor));

        var sobrenomeNormalizado = valor.Trim();

        if (sobrenomeNormalizado.Length < 3)
            throw new ArgumentException("O sobrenome deve ter no mínimo 3 caracteres.", nameof(valor));

        if (sobrenomeNormalizado.Length > 50)
            throw new ArgumentException("O sobrenome deve ter no máximo 50 caracteres.", nameof(valor));

        Valor = sobrenomeNormalizado;
    }

    /// <summary>
    /// Retorna a representação em string do sobrenome.
    /// </summary>
    /// <returns>O valor do sobrenome.</returns>
    public override string ToString() => Valor;

    /// <summary>
    /// Conversão implícita de Sobrenome para string.
    /// Permite usar Sobrenome diretamente onde string é esperado.
    /// </summary>
    /// <param name="sobrenome">O Sobrenome a ser convertido.</param>
    public static implicit operator string(Sobrenome sobrenome) => sobrenome.Valor;
}

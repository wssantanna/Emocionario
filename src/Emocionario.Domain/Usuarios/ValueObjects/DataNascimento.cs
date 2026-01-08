namespace Emocionario.Domain.Usuarios.ValueObjects;

/// <summary>
/// Value Object que representa a data de nascimento de um usuário.
/// Garante a imutabilidade e as regras de validação para datas de nascimento.
/// </summary>
/// <remarks>
/// Implementado como readonly record struct para garantir:
/// - Imutabilidade: uma vez criado, o valor não pode ser alterado
/// - Igualdade por valor: dois DataNascimento são iguais se possuem o mesmo valor
/// - Performance: struct evita alocação no heap
///
/// Regras de validação:
/// - A data não pode ser no futuro (deve ser menor ou igual à data atual em UTC)
/// - Utiliza DateOnly para representar apenas a data, sem hora
///
/// Formatação:
/// - ToString() retorna a data no formato ISO 8601: "yyyy-MM-dd"
/// </remarks>
public readonly record struct DataNascimento
{
    /// <summary>
    /// Obtém o valor da data de nascimento.
    /// </summary>
    public DateOnly Valor { get; }

    /// <summary>
    /// Inicializa uma nova instância de DataNascimento com validações.
    /// </summary>
    /// <param name="valor">A data de nascimento a ser encapsulada.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando a data de nascimento é posterior à data atual (data futura).
    /// A validação é feita contra a data atual em UTC.
    /// </exception>
    public DataNascimento(DateOnly valor)
    {
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);

        // Validação: a data de nascimento não pode ser no futuro
        if (valor > hoje)
            throw new ArgumentException("A data de nascimento não pode ser em uma data futura.", nameof(valor));

        Valor = valor;
    }

    /// <summary>
    /// Retorna a representação em string da data de nascimento no formato ISO 8601 (yyyy-MM-dd).
    /// </summary>
    /// <returns>A data de nascimento formatada como "yyyy-MM-dd".</returns>
    /// <example>
    /// Para a data 15 de março de 1990, retorna: "1990-03-15"
    /// </example>
    public override string ToString() => Valor.ToString("yyyy-MM-dd");

    /// <summary>
    /// Conversão implícita de DataNascimento para DateOnly.
    /// Permite usar DataNascimento diretamente onde DateOnly é esperado.
    /// </summary>
    /// <param name="dataNascimento">O DataNascimento a ser convertido.</param>
    public static implicit operator DateOnly(DataNascimento dataNascimento) => dataNascimento.Valor;
}

namespace Emocionario.Domain.Usuarios.ValueObjects;
public readonly record struct DataNascimento
{
    public DateOnly Valor { get; }
    public DataNascimento(DateOnly valor)
    {
        if (valor > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("A data de nascimento não pode ser em uma data futura.", nameof(valor));

        Valor = valor;
    }
    public override string ToString() => Valor.ToString("yyyy-MM-dd");
    public static implicit operator DateOnly(DataNascimento dataNascimento) => dataNascimento.Valor;
}
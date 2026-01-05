namespace Emocionario.Domain.Usuarios.ValueObjects;
public readonly record struct PrimeiroNome
{
    public string Valor { get; }
    public PrimeiroNome(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("O primeiro nome não pode estar vazio.", nameof(valor));
        if (valor.Length < 3)
            throw new ArgumentException("O primeiro nome deve ter no mínimo 3 caracteres.", nameof(valor));
        if (valor.Length > 50)
            throw new ArgumentException("O primeiro nome deve ter no máximo 50 caracteres.", nameof(valor));

        Valor = valor;
    }
    public override string ToString() => Valor;
    public static implicit operator string(PrimeiroNome primeiroNome) => primeiroNome.Valor;
}
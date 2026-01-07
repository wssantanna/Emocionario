namespace Emocionario.Domain.Usuarios.ValueObjects;
public readonly record struct Email
{
    public string Valor { get; }
    public Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("O email não pode estar vazio.", nameof(valor));

        var emailNormalizado = valor.Trim().ToLowerInvariant();

        if (!emailNormalizado.Contains("@") || !emailNormalizado.Contains("."))
            throw new ArgumentException("O email fornecido não é válido.", nameof(valor));

        Valor = emailNormalizado;
    }
    public override string ToString() => Valor;
    public static implicit operator string(Email email) => email.Valor;
}
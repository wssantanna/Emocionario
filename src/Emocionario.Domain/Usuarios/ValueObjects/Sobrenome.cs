namespace Emocionario.Domain.Usuarios.ValueObjects;
public readonly record struct Sobrenome
{
    public string Valor { get; }
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
    public override string ToString() => Valor;
    public static implicit operator string(Sobrenome sobrenome) => sobrenome.Valor;
}
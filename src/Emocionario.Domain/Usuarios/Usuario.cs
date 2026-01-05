using Emocionario.Domain.Usuarios.ValueObjects;
public sealed class Usuario {
    public Guid Id { get; private set; }
    public PrimeiroNome Nome { get; private set; }
    public Sobrenome Sobrenome { get; private set; }
    public Email Email { get; private set; }
    public DataNascimento DataNascimento { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }
    private Usuario() { }
    public static Usuario Criar(PrimeiroNome nome, Sobrenome sobrenome, Email email, DataNascimento dataNascimento)
    {
        return new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Sobrenome = sobrenome,
            Email = email,
            DataNascimento = dataNascimento,
            CriadoEm = DateTime.UtcNow
        };
    }
    public void Atualizar(PrimeiroNome nome, Sobrenome sobrenome, DataNascimento dataNascimento)
    {
        Nome = nome;
        Sobrenome = sobrenome;
        DataNascimento = dataNascimento;
        AtualizadoEm = DateTime.UtcNow;
    }
}
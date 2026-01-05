public sealed class Usuario {
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Sobrenome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateOnly DataNascimento { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }
    private Usuario() { }
    public static Usuario Criar(string nome, string sobrenome, string email, DateOnly dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O primeiro nome não pode estar vazio.", nameof(nome));
        if (string.IsNullOrWhiteSpace(sobrenome))
            throw new ArgumentException("O sobrenome não pode estar vazio.", nameof(sobrenome));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O email não pode estar vazio.", nameof(email));
        if (dataNascimento > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("A data de nascimento não pode ser no futuro.", nameof(dataNascimento));

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
    public void Atualizar(string nome, string sobrenome, DateOnly dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome não pode estar vazio.", nameof(nome));
        if (nome.Length < 3)
            throw new ArgumentException("O nome deve ter no mínimo 3 caracteres.", nameof(nome));
        if (nome.Length > 50)
            throw new ArgumentException("O nome deve ter no máximo 50 caracteres.", nameof(nome));
        if (string.IsNullOrWhiteSpace(sobrenome))
            throw new ArgumentException("O sobrenome não pode estar vazio.", nameof(sobrenome));
        if (sobrenome.Length < 3)
            throw new ArgumentException("O sobrenome deve ter no mínimo 3 caracteres.", nameof(sobrenome));
        if (sobrenome.Length > 50)
            throw new ArgumentException("O sobrenome deve ter no máximo 50 caracteres.", nameof(sobrenome));
        if (dataNascimento > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("A data de nascimento não pode ser uma data futura.", nameof(dataNascimento));

        Nome = nome;
        Sobrenome = sobrenome;
        DataNascimento = dataNascimento;
        AtualizadoEm = DateTime.UtcNow;
    }
}
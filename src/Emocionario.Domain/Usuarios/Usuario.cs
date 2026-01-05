public sealed class Usuario {
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Sobrenome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateOnly DataNascimento { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }
}
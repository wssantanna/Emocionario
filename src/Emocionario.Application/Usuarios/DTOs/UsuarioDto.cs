namespace Emocionario.Application.Usuarios.DTOs;
public sealed record UsuarioDto
{
    public required Guid Id { get; init; }
    public required string Nome { get; init; }
    public required string Sobrenome { get; init; }
    public required string Email { get; init; }
    public DateTime? DataNascimento { get; init; }
    public required DateTime CriadoEm { get; init; }
    public DateTime? AtualizadoEm { get; init; }
}

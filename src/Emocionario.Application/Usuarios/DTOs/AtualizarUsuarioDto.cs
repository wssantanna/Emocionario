namespace Emocionario.Application.Usuarios.DTOs;
public sealed record AtualizarUsuarioDto
{
    public required Guid Id { get; init; }
    public string? Nome { get; init; }
    public string? Sobrenome { get; init; }
    public DateTime? DataNascimento { get; init; }
}

namespace Emocionario.Application.Usuarios.DTOs;
public sealed record CriarUsuarioDto
{
    public required string Nome { get; init; }
    public required string Sobrenome { get; init; }
    public required string Email { get; init; }
    public DateOnly? DataNascimento { get; init; }
}

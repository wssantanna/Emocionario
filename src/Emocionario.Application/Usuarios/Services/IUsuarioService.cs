using Emocionario.Application.Usuarios.DTOs;
namespace Emocionario.Application.Usuarios.Services;
public interface IUsuarioService
{
    Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto, CancellationToken cancellationToken = default);
    Task<UsuarioDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UsuarioDto?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> AtualizarAsync(AtualizarUsuarioDto dto, CancellationToken cancellationToken = default);
    Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default);
}

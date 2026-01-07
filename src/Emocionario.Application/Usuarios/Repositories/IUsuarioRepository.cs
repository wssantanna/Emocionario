using Emocionario.Domain.Usuarios;
using Emocionario.Domain.Usuarios.ValueObjects;
namespace Emocionario.Application;
public interface IUsuarioRepository
{
    Task<bool> ExisteComEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<Usuario> AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<bool> AtualizarAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default);
}

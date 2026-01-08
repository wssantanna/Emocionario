using Emocionario.Application.Usuarios.Repositories;
using Emocionario.Domain.Usuarios;
using Emocionario.Domain.Usuarios.ValueObjects;
using Emocionario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Emocionario.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly EmocionarioDbContext _context;

    public UsuarioRepository(EmocionarioDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExisteComEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        await _context.Usuarios.AddAsync(usuario, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<Usuario?> ObterPorEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task AtualizarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usuario = await ObterPorIdAsync(id, cancellationToken);

        if (usuario is null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

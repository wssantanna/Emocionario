using Emocionario.Application.Usuarios.DTOs;
using Emocionario.Application.Usuarios.Repositories;
using Emocionario.Domain.Usuarios;
using Emocionario.Domain.Usuarios.ValueObjects;
namespace Emocionario.Application.Usuarios.Services;
public sealed class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        var email = new Email(dto.Email);

        if (await _usuarioRepository.ExisteComEmailAsync(email, cancellationToken))
            throw new InvalidOperationException($"Já existe um usuário cadastrado com o email '{dto.Email}'.");

        var nome = new PrimeiroNome(dto.Nome);
        var sobrenome = new Sobrenome(dto.Sobrenome);
        DataNascimento? dataNascimento = dto.DataNascimento.HasValue
            ? new DataNascimento(DateOnly.FromDateTime(dto.DataNascimento.Value))
            : null;

        var usuario = Usuario.Criar(nome, sobrenome, email, dataNascimento);

        await _usuarioRepository.AdicionarAsync(usuario, cancellationToken);

        return MapearParaDto(usuario);
    }

    public async Task<UsuarioDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id, cancellationToken);
        return usuario is not null ? MapearParaDto(usuario) : null;
    }

    public async Task<UsuarioDto?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var emailVo = new Email(email);
        var usuario = await _usuarioRepository.ObterPorEmailAsync(emailVo, cancellationToken);
        return usuario is not null ? MapearParaDto(usuario) : null;
    }

    public async Task<bool> AtualizarAsync(AtualizarUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(dto.Id, cancellationToken);

        if (usuario is null)
            return false;

        var nome = !string.IsNullOrWhiteSpace(dto.Nome)
            ? new PrimeiroNome(dto.Nome)
            : usuario.Nome;

        var sobrenome = !string.IsNullOrWhiteSpace(dto.Sobrenome)
            ? new Sobrenome(dto.Sobrenome)
            : usuario.Sobrenome;

        DataNascimento? dataNascimento = dto.DataNascimento.HasValue
            ? new DataNascimento(DateOnly.FromDateTime(dto.DataNascimento.Value))
            : usuario.DataNascimento;

        usuario.Atualizar(nome, sobrenome, dataNascimento);

        await _usuarioRepository.AtualizarAsync(usuario, cancellationToken);

        return true;
    }

    public async Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _usuarioRepository.ExcluirAsync(id, cancellationToken);
    }

    private static UsuarioDto MapearParaDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Sobrenome = usuario.Sobrenome,
            Email = usuario.Email,
            DataNascimento = usuario.DataNascimento?.Valor.ToDateTime(TimeOnly.MinValue),
            CriadoEm = usuario.CriadoEm,
            AtualizadoEm = usuario.AtualizadoEm
        };
    }
}

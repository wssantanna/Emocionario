using Emocionario.Application.Usuarios.DTOs;
using Emocionario.Application.Usuarios.Repositories;
using Emocionario.Domain.Usuarios;
using Emocionario.Domain.Usuarios.ValueObjects;

namespace Emocionario.Application.Usuarios.Services;

/// <summary>
/// Implementação do serviço de aplicação para operações relacionadas a usuários.
/// Orquestra a interação entre DTOs, entidades de domínio e o repositório.
/// </summary>
/// <remarks>
/// Esta classe implementa a camada de aplicação seguindo os princípios de Clean Architecture:
/// - Converte DTOs (dados primitivos) em Value Objects e Entidades de domínio
/// - Aplica regras de negócio da aplicação (como validação de unicidade de e-mail)
/// - Delega operações de persistência para o repositório
/// - Mantém a separação entre a camada de apresentação e o domínio
/// - Utiliza o padrão sealed para prevenir herança indesejada
/// </remarks>
public sealed class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    /// <summary>
    /// Inicializa uma nova instância do serviço de usuários.
    /// </summary>
    /// <param name="usuarioRepository">Repositório para acesso aos dados de usuários.</param>
    /// <exception cref="ArgumentNullException">Lançada quando usuarioRepository é null.</exception>
    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Cria um novo usuário no sistema.
    /// </summary>
    /// <param name="dto">DTO contendo os dados do usuário a ser criado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o DTO do usuário criado, incluindo o ID gerado e a data de criação.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Lançada quando já existe um usuário cadastrado com o e-mail fornecido.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados violam as regras de validação dos Value Objects.
    /// </exception>
    public async Task<UsuarioDto> CriarAsync(CriarUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        // Converte o e-mail para Value Object e valida o formato
        var email = new Email(dto.Email);

        // Regra de negócio: validação de unicidade do e-mail (Fail-Fast)
        if (await _usuarioRepository.ExisteComEmailAsync(email, cancellationToken))
            throw new InvalidOperationException($"Já existe um usuário cadastrado com o email '{dto.Email}'.");

        // Converte dados do DTO em Value Objects do domínio
        var nome = new PrimeiroNome(dto.Nome);
        var sobrenome = new Sobrenome(dto.Sobrenome);
        DataNascimento? dataNascimento = dto.DataNascimento.HasValue
            ? new DataNascimento(dto.DataNascimento.Value)
            : null;

        // Cria a entidade usando o Factory Method do domínio
        var usuario = Usuario.Criar(nome, sobrenome, email, dataNascimento);

        // Persiste o usuário
        await _usuarioRepository.AdicionarAsync(usuario, cancellationToken);

        // Retorna o DTO mapeado
        return MapearParaDto(usuario);
    }

    /// <summary>
    /// Obtém um usuário pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único (GUID) do usuário.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o DTO do usuário encontrado ou null se não existir.
    /// </returns>
    public async Task<UsuarioDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id, cancellationToken);
        return usuario is not null ? MapearParaDto(usuario) : null;
    }

    /// <summary>
    /// Obtém um usuário pelo seu endereço de e-mail.
    /// </summary>
    /// <param name="email">O endereço de e-mail do usuário.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém o DTO do usuário encontrado ou null se não existir.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o formato do e-mail é inválido.
    /// </exception>
    public async Task<UsuarioDto?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        // Converte string para Value Object Email (aplica validação e normalização)
        var emailVo = new Email(email);
        var usuario = await _usuarioRepository.ObterPorEmailAsync(emailVo, cancellationToken);
        return usuario is not null ? MapearParaDto(usuario) : null;
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// Suporta atualização parcial: apenas campos fornecidos serão atualizados.
    /// </summary>
    /// <param name="dto">DTO contendo o ID e os campos a serem atualizados.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém true se o usuário foi atualizado; false se não foi encontrado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando os novos dados violam as regras de validação dos Value Objects.
    /// </exception>
    public async Task<bool> AtualizarAsync(AtualizarUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(dto.Id, cancellationToken);

        if (usuario is null)
            return false;

        // Atualização parcial: se o campo não foi fornecido, mantém o valor atual
        var nome = !string.IsNullOrWhiteSpace(dto.Nome)
            ? new PrimeiroNome(dto.Nome)
            : usuario.Nome;

        var sobrenome = !string.IsNullOrWhiteSpace(dto.Sobrenome)
            ? new Sobrenome(dto.Sobrenome)
            : usuario.Sobrenome;

        DataNascimento? dataNascimento = dto.DataNascimento.HasValue
            ? new DataNascimento(dto.DataNascimento.Value)
            : usuario.DataNascimento;

        // Atualiza a entidade (a data AtualizadoEm é definida automaticamente)
        usuario.Atualizar(nome, sobrenome, dataNascimento);

        await _usuarioRepository.AtualizarAsync(usuario, cancellationToken);

        return true;
    }

    /// <summary>
    /// Remove um usuário do sistema pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único (GUID) do usuário a ser excluído.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
    /// <returns>
    /// Uma task que representa a operação assíncrona.
    /// O resultado contém true se o usuário foi excluído; false se não foi encontrado.
    /// </returns>
    public async Task<bool> ExcluirAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _usuarioRepository.ExcluirAsync(id, cancellationToken);
    }

    /// <summary>
    /// Mapeia uma entidade de domínio Usuario para o DTO UsuarioDto.
    /// </summary>
    /// <param name="usuario">A entidade Usuario do domínio.</param>
    /// <returns>O DTO com os dados do usuário para a camada de apresentação.</returns>
    /// <remarks>
    /// Este método é privado pois é um detalhe de implementação do serviço.
    /// Os Value Objects são automaticamente convertidos para seus tipos primitivos
    /// através dos operadores de conversão implícita definidos em cada Value Object.
    /// </remarks>
    private static UsuarioDto MapearParaDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,              // Conversão implícita de PrimeiroNome para string
            Sobrenome = usuario.Sobrenome,    // Conversão implícita de Sobrenome para string
            Email = usuario.Email,            // Conversão implícita de Email para string
            DataNascimento = usuario.DataNascimento?.Valor,
            CriadoEm = usuario.CriadoEm,
            AtualizadoEm = usuario.AtualizadoEm
        };
    }
}

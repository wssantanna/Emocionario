using Emocionario.Domain.Usuarios.ValueObjects;

namespace Emocionario.Domain.Usuarios;

/// <summary>
/// Representa a entidade raiz do agregado de Usuário no domínio.
/// Encapsula as informações principais de identificação e dados pessoais do usuário.
/// </summary>
/// <remarks>
/// Esta classe é selada (sealed) para evitar herança e mantém a integridade do agregado.
/// Utiliza o padrão Factory Method através do método Criar() para garantir a criação válida de instâncias.
/// </remarks>
public sealed class Usuario
{
    /// <summary>
    /// Identificador único do usuário.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Primeiro nome do usuário, encapsulado como Value Object.
    /// </summary>
    public PrimeiroNome Nome { get; private set; }

    /// <summary>
    /// Sobrenome do usuário, encapsulado como Value Object.
    /// </summary>
    public Sobrenome Sobrenome { get; private set; }

    /// <summary>
    /// Endereço de e-mail do usuário, encapsulado como Value Object.
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// Data de nascimento do usuário, encapsulada como Value Object.
    /// É opcional (nullable) pois nem sempre é obrigatório coletar esta informação.
    /// </summary>
    public DataNascimento? DataNascimento { get; private set; }

    /// <summary>
    /// Data e hora (UTC) em que o usuário foi criado no sistema.
    /// </summary>
    public DateTime CriadoEm { get; private set; }

    /// <summary>
    /// Data e hora (UTC) da última atualização dos dados do usuário.
    /// É nulo quando o usuário nunca foi atualizado.
    /// </summary>
    public DateTime? AtualizadoEm { get; private set; }

    /// <summary>
    /// Construtor privado para garantir que a criação de usuários ocorra apenas através do método Criar().
    /// Previne a instanciação direta e assegura que todas as validações sejam executadas.
    /// </summary>
    private Usuario() { }

    /// <summary>
    /// Factory Method para criar uma nova instância de Usuário.
    /// </summary>
    /// <param name="nome">Primeiro nome do usuário.</param>
    /// <param name="sobrenome">Sobrenome do usuário.</param>
    /// <param name="email">Endereço de e-mail do usuário.</param>
    /// <param name="dataNascimento">Data de nascimento do usuário (opcional).</param>
    /// <returns>Uma nova instância de Usuário com identificador único gerado.</returns>
    /// <remarks>
    /// Este método garante que:
    /// - Um novo GUID seja gerado automaticamente
    /// - A data de criação seja registrada em UTC
    /// - Todas as validações dos Value Objects sejam executadas
    /// </remarks>
    public static Usuario Criar(PrimeiroNome nome, Sobrenome sobrenome, Email email, DataNascimento? dataNascimento = null)
    {
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

    /// <summary>
    /// Atualiza as informações do usuário.
    /// </summary>
    /// <param name="nome">Novo primeiro nome do usuário.</param>
    /// <param name="sobrenome">Novo sobrenome do usuário.</param>
    /// <param name="dataNascimento">Nova data de nascimento do usuário (opcional).</param>
    /// <remarks>
    /// Este método:
    /// - Não permite a atualização do e-mail (por design, email é imutável após criação)
    /// - Registra automaticamente a data/hora da atualização em UTC
    /// - Mantém as validações dos Value Objects
    /// </remarks>
    public void Atualizar(PrimeiroNome nome, Sobrenome sobrenome, DataNascimento? dataNascimento)
    {
        Nome = nome;
        Sobrenome = sobrenome;
        DataNascimento = dataNascimento;
        AtualizadoEm = DateTime.UtcNow;
    }
}

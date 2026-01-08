using Emocionario.Application.Usuarios.Repositories;
using Emocionario.Infrastructure.Data;
using Emocionario.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Emocionario.Infrastructure;

/// <summary>
/// Classe estática responsável pela configuração de injeção de dependências da camada de infraestrutura.
/// Centraliza o registro de todos os serviços relacionados a persistência de dados.
/// </summary>
/// <remarks>
/// Esta classe implementa o padrão Extension Method para IServiceCollection,
/// permitindo configurar toda a infraestrutura com uma única chamada: services.AddInfrastructure().
///
/// Serviços registrados:
/// - DbContext: EmocionarioDbContext (Scoped) com InMemory Database
/// - Repositories: IUsuarioRepository → UsuarioRepository (Scoped)
///
/// Lifecycle dos serviços:
/// - Scoped: uma instância por request HTTP (recomendado para DbContext e Repositories)
/// </remarks>
public static class DependencyInjection
{
    /// <summary>
    /// Adiciona e configura os serviços de infraestrutura no container de injeção de dependências.
    /// </summary>
    /// <param name="services">A coleção de serviços onde os componentes de infraestrutura serão registrados.</param>
    /// <returns>A mesma coleção de serviços para permitir chamadas encadeadas (fluent API).</returns>
    /// <remarks>
    /// Este método deve ser chamado no Program.cs ou Startup.cs durante a configuração da aplicação.
    ///
    /// Exemplo de uso:
    /// <code>
    /// var builder = WebApplication.CreateBuilder(args);
    /// builder.Services.AddInfrastructure();
    /// </code>
    ///
    /// Atualmente utiliza InMemory Database para desenvolvimento.
    /// Para produção, substitua UseInMemoryDatabase por UseSqlServer ou UseNpgsql.
    /// </remarks>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Configuração do DbContext com InMemory Database para desenvolvimento/testes
        // TODO: Em produção, substituir por UseSqlServer ou UseNpgsql com connection string
        services.AddDbContext<EmocionarioDbContext>(options =>
            options.UseInMemoryDatabase("EmocionarioDB"));

        // Registro dos Repositories seguindo o padrão Repository Pattern
        // Lifetime: Scoped (uma instância por request HTTP)
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}

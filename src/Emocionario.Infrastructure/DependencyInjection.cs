using Emocionario.Application.Usuarios.Repositories;
using Emocionario.Infrastructure.Data;
using Emocionario.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Emocionario.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Configuração do DbContext com InMemory Database
        services.AddDbContext<EmocionarioDbContext>(options =>
            options.UseInMemoryDatabase("EmocionarioDB"));

        // Registro dos Repositories
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}

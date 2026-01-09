using Emocionario.Application.Usuarios.DTOs;
using Emocionario.Application.Usuarios.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Emocionario.Api.Endpoints;

/// <summary>
/// Define os endpoints HTTP para operações de usuários utilizando Minimal APIs.
/// </summary>
public static class UsuariosEndpoints
{
    /// <summary>
    /// Mapeia todos os endpoints de usuários.
    /// </summary>
    public static void MapUsuariosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/usuarios")
            .WithTags("Usuários")
            .WithOpenApi();

        group.MapPost("/", CriarUsuario)
            .WithName("CriarUsuario")
            .WithSummary("Cria um novo usuário")
            .Produces<UsuarioDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapGet("/{id:guid}", ObterUsuarioPorId)
            .WithName("ObterUsuarioPorId")
            .WithSummary("Obtém um usuário por ID")
            .Produces<UsuarioDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/email/{email}", ObterUsuarioPorEmail)
            .WithName("ObterUsuarioPorEmail")
            .WithSummary("Obtém um usuário por email")
            .Produces<UsuarioDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}", AtualizarUsuario)
            .WithName("AtualizarUsuario")
            .WithSummary("Atualiza os dados de um usuário")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", ExcluirUsuario)
            .WithName("ExcluirUsuario")
            .WithSummary("Exclui um usuário")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Cria um novo usuário no sistema.
    /// </summary>
    private static async Task<IResult> CriarUsuario(
        [FromBody] CriarUsuarioDto dto,
        [FromServices] IUsuarioService usuarioService,
        [FromServices] IValidator<CriarUsuarioDto> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        try
        {
            var usuario = await usuarioService.CriarAsync(dto, cancellationToken);
            return Results.Created($"/api/usuarios/{usuario.Id}", usuario);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Problem(
                title: "Operação inválida",
                detail: ex.Message,
                statusCode: StatusCodes.Status409Conflict
            );
        }
        catch (ArgumentException ex)
        {
            return Results.Problem(
                title: "Argumento inválido",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
    }

    /// <summary>
    /// Obtém um usuário pelo ID.
    /// </summary>
    private static async Task<IResult> ObterUsuarioPorId(
        [FromRoute] Guid id,
        [FromServices] IUsuarioService usuarioService,
        CancellationToken cancellationToken)
    {
        var usuario = await usuarioService.ObterPorIdAsync(id, cancellationToken);

        return usuario is not null
            ? Results.Ok(usuario)
            : Results.Problem(
                title: "Usuário não encontrado",
                detail: $"Nenhum usuário encontrado com o ID {id}.",
                statusCode: StatusCodes.Status404NotFound
            );
    }

    /// <summary>
    /// Obtém um usuário pelo email.
    /// </summary>
    private static async Task<IResult> ObterUsuarioPorEmail(
        [FromRoute] string email,
        [FromServices] IUsuarioService usuarioService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Results.Problem(
                title: "Email inválido",
                detail: "O email fornecido não pode ser vazio.",
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        var usuario = await usuarioService.ObterPorEmailAsync(email, cancellationToken);

        return usuario is not null
            ? Results.Ok(usuario)
            : Results.Problem(
                title: "Usuário não encontrado",
                detail: $"Nenhum usuário encontrado com o email '{email}'.",
                statusCode: StatusCodes.Status404NotFound
            );
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// </summary>
    private static async Task<IResult> AtualizarUsuario(
        [FromRoute] Guid id,
        [FromBody] AtualizarUsuarioDto dto,
        [FromServices] IUsuarioService usuarioService,
        [FromServices] IValidator<AtualizarUsuarioDto> validator,
        CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return Results.Problem(
                title: "ID incompatível",
                detail: "O ID da rota não corresponde ao ID no corpo da requisição.",
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        try
        {
            var atualizado = await usuarioService.AtualizarAsync(dto, cancellationToken);

            return atualizado
                ? Results.NoContent()
                : Results.Problem(
                    title: "Usuário não encontrado",
                    detail: $"Nenhum usuário encontrado com o ID {id}.",
                    statusCode: StatusCodes.Status404NotFound
                );
        }
        catch (ArgumentException ex)
        {
            return Results.Problem(
                title: "Argumento inválido",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest
            );
        }
    }

    /// <summary>
    /// Exclui um usuário do sistema.
    /// </summary>
    private static async Task<IResult> ExcluirUsuario(
        [FromRoute] Guid id,
        [FromServices] IUsuarioService usuarioService,
        CancellationToken cancellationToken)
    {
        var excluido = await usuarioService.ExcluirAsync(id, cancellationToken);

        return excluido
            ? Results.NoContent()
            : Results.Problem(
                title: "Usuário não encontrado",
                detail: $"Nenhum usuário encontrado com o ID {id}.",
                statusCode: StatusCodes.Status404NotFound
            );
    }
}

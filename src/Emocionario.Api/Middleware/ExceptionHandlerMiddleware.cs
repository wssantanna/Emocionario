using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Emocionario.Api.Middleware;

/// <summary>
/// Middleware para tratamento global de exceções.
/// Captura todas as exceções não tratadas e retorna respostas HTTP apropriadas.
/// </summary>
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu uma exceção não tratada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ArgumentException argEx => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Argumento inválido",
                argEx.Message
            ),
            ValidationException valEx => CreateValidationErrorResponse(
                HttpStatusCode.BadRequest,
                "Erro de validação",
                valEx.Errors.Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage })
            ),
            KeyNotFoundException notFoundEx => CreateErrorResponse(
                HttpStatusCode.NotFound,
                "Recurso não encontrado",
                notFoundEx.Message
            ),
            InvalidOperationException invOpEx => CreateErrorResponse(
                HttpStatusCode.Conflict,
                "Operação inválida",
                invOpEx.Message
            ),
            _ => CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                "Erro interno do servidor",
                "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."
            )
        };

        context.Response.StatusCode = response.StatusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response.Body, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }

    private static (int StatusCode, object Body) CreateErrorResponse(HttpStatusCode statusCode, string title, string detail)
    {
        return ((int)statusCode, new
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = title,
            Status = (int)statusCode,
            Detail = detail,
            Timestamp = DateTime.UtcNow
        });
    }

    private static (int StatusCode, object Body) CreateValidationErrorResponse(HttpStatusCode statusCode, string title, object errors)
    {
        return ((int)statusCode, new
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = title,
            Status = (int)statusCode,
            Errors = errors,
            Timestamp = DateTime.UtcNow
        });
    }
}
